using UnityEngine;

[System.Serializable]
public class GameProgressionService : IService
{
    public int AlianceCredits;
    public int DeAthomizerBooster;
    public int Dilithium;
    public int EasyTriggerBooster;
    public int FistAidKitBooster;
    public string[] StarshipColors;
    public string[] StarshipModel;

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

        AlianceCredits = config.PlayerInitialAlianceCredits;
        DeAthomizerBooster = config.PlayerInitialDeAthomizerBooster;
        Dilithium = config.PlayerInitialDilithium;
        EasyTriggerBooster = config.PlayerInitialEasyTriggerBooster;
        FistAidKitBooster = config.PlayerInitialFistAidKitBooster;
        StarshipColors = config.PlayerInitialStarshipColors.Split("-");
        StarshipModel = config.PlayerInitialStarshipModel.Split("-");

        Save();
    }

    public void Clear() { }
}
