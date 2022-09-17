using UnityEngine;
using System.IO;
public class SaveLoadService : IService
{
    private static string kSavePath = Application.persistentDataPath + Constants.SaveFileName;
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

        _gameProgression.UpdateElement(Constants.AllianceCredits, config.PlayerInitialAllianceCredits);
        _gameProgression.UpdateElement(Constants.Dilithium, config.PlayerInitialDilithium);
        _gameProgression.UpdateElement(Constants.DeAthomizer, config.PlayerInitialDeAthomizerBooster);
        _gameProgression.UpdateElement(Constants.EasyTrigger, config.PlayerInitialEasyTriggerBooster);
        _gameProgression.UpdateElement(Constants.FirstAidKit, config.PlayerInitialFistAidKitBooster);
        _gameProgression.UnlockStarshipModel(config.PlayerInitialStarshipModel);
        _gameProgression.UnlockColorPack(config.PlayerInitialStarshipColors);

        PlayerPrefs.SetString(Constants.EquipedStarshipModel, config.PlayerInitialStarshipModel);
        PlayerPrefs.SetString(Constants.EquipedStarshipColors, config.PlayerInitialStarshipColors);

        Save();
    }

    public void DeleteLocalFiles()
    {
        if (File.Exists(kSavePath)) 
            File.Delete(kSavePath);
    }
    public void Clear() { }
}