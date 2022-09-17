using UnityEngine;
using System.IO;
public class SaveLoadService : IService
{
    private static string kSavePath = Application.persistentDataPath + "/_gameProgression.json";
    [SerializeField] private GameProgressionService _gameProgression;
    public void Initialize(GameConfigService config, GameProgressionService gameProgression) 
    {
        _gameProgression = gameProgression;

        Load(config);
    }
    public void Save() => File.WriteAllText(kSavePath, JsonUtility.ToJson(_gameProgression));

    private void Load(GameConfigService config)
    {
        if (File.Exists(kSavePath))
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(kSavePath), _gameProgression);
            return;
        }

        _gameProgression.UpdateElement("AllianceCredits", config.PlayerInitialAllianceCredits);
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

    public void DeleteLocalFiles()
    {
        if (File.Exists(kSavePath)) 
            File.Delete(kSavePath);
    }
    public void Clear() { }
}