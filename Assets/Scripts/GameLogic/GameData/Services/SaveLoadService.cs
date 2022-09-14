using UnityEngine;

public class SaveLoadService : IService
{
    private static string kSavePath = Application.persistentDataPath + "/_gameProgression.json";
    [SerializeField] private GameProgressionService _gameProgression;
    public void Initialize(GameConfigService config, GameProgressionService gameProgression) 
    {
        _gameProgression = gameProgression;

        Load(config);
    }
    public void Save() => System.IO.File.WriteAllText(kSavePath, JsonUtility.ToJson(_gameProgression));

    private void Load(GameConfigService config)
    {
        if (System.IO.File.Exists(kSavePath))
        {
            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(kSavePath), _gameProgression);
            return;
        }

        _gameProgression.UpdateElement("AlianceCredits", config.PlayerInitialAlianceCredits);
        _gameProgression.UpdateElement("Dilithium", config.PlayerInitialDilithium);
        _gameProgression.UpdateElement("DeAthomizer", config.PlayerInitialDeAthomizerBooster);
        _gameProgression.UpdateElement("EasyTrigger", config.PlayerInitialEasyTriggerBooster);
        _gameProgression.UpdateElement("FirstAidKit", config.PlayerInitialFistAidKitBooster);
        _gameProgression.UnlockStarshipModel(config.PlayerInitialStarshipModel);
        _gameProgression.UnlockColorPack(config.PlayerInitialStarshipColors);

        PlayerPrefs.SetString("EquipedStarshipModel", config.PlayerInitialStarshipModel);
        PlayerPrefs.SetString("EquipedStarshipColors", config.PlayerInitialStarshipColors);

        Save();
    }
    public void Clear() { }
}