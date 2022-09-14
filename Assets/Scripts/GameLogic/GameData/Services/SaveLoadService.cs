using UnityEngine;

public class SaveLoadService : IService
{
    private static string kSavePath = Application.persistentDataPath + "/_gameProgression.json";
    public void Initialize(GameConfigService config, GameProgressionService progression) 
    {
        Load(config, progression);
    }
    public void Save() => System.IO.File.WriteAllText(kSavePath, JsonUtility.ToJson(this));

    private void Load(GameConfigService config, GameProgressionService progression)
    {
        if (System.IO.File.Exists(kSavePath))
        {
            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(kSavePath), this);
            return;
        }

        progression.UpdateElement("AlianceCredits", config.PlayerInitialAlianceCredits);
        progression.UpdateElement("Dilithium", config.PlayerInitialDilithium);
        progression.UpdateElement("DeAthomizer", config.PlayerInitialDeAthomizerBooster);
        progression.UpdateElement("EasyTrigger", config.PlayerInitialEasyTriggerBooster);
        progression.UpdateElement("FirstAidKit", config.PlayerInitialFistAidKitBooster);

        progression.UnlockStarshipModel(config.PlayerInitialStarshipModel);
        progression.UnlockColorPack(config.PlayerInitialStarshipColors);

        PlayerPrefs.SetString("EquipedStarshipModel", config.PlayerInitialStarshipModel);
        PlayerPrefs.SetString("EquipedStarshipColors", config.PlayerInitialStarshipColors);

        Save();
    }
    public void Clear() { }
}