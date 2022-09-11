using UnityEngine;

[System.Serializable]
public class GameProgressionService : IService
{
    public int PlayerInitialAlianceCredits;
    public int PlayerInitialDeAthomizerBooster;
    public int PlayerInitialDilithium;
    public int PlayerInitialEasyTriggerBooster;
    public int PlayerInitialFistAidKitBooster;
    public string PlayerInitialStarshipColors;
    public string PlayerInitialStarshipModel;

    private static string kSavePath = Application.persistentDataPath + "/gameProgression.json";

    public void Save()
    {
        System.IO.File.WriteAllText(kSavePath, JsonUtility.ToJson(this));
    }

    public void Load(GameConfigService config)
    {
        if (System.IO.File.Exists(kSavePath))
        {
            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(kSavePath), this);
            return;
        }

        PlayerInitialAlianceCredits = config.PlayerInitialAlianceCredits;
        PlayerInitialDeAthomizerBooster = config.PlayerInitialDeAthomizerBooster;
        PlayerInitialDilithium = config.PlayerInitialDilithium;
        PlayerInitialEasyTriggerBooster = config.PlayerInitialEasyTriggerBooster;
        PlayerInitialFistAidKitBooster = config.PlayerInitialFistAidKitBooster;
        PlayerInitialStarshipColors = config.PlayerInitialStarshipColors;
        PlayerInitialStarshipModel = config.PlayerInitialStarshipModel;

        Save();
    }

    public void Clear() { }
}
