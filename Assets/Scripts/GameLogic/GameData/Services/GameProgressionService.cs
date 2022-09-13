using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameProgressionService : IService
{
    [SerializeField] private int _alianceCredits;
    [SerializeField] private int _dilithium;
    [SerializeField] private int _deAthomizerBooster;
    [SerializeField] private int _easyTriggerBooster;
    [SerializeField] private int _fistAidKitBooster;
    [SerializeField] private string _starshipModel;
    [SerializeField] private List<string> _starshipColors;

    public int AlianceCredits => _alianceCredits;
    public int Dilithium => _dilithium;
    public int DeAthomizerBooster => _deAthomizerBooster;
    public int EasyTriggerBooster => _easyTriggerBooster;
    public int FistAidKitBooster => _fistAidKitBooster;
    public string StarshipModel => _starshipModel;
    public List<string> StarshipColors => _starshipColors;

    public void UpdateAlianceCredits(int amount)
    {
        _alianceCredits += amount;
        Save();
        //UICahge event
        //...
    }
    public void UpdateDilithium(int amount)
    {
        _dilithium += amount;
        Save();
        //UICahge event
        //...
    }
    public void UpdateDeAthomizerBooster(int amount)
    {
        _deAthomizerBooster += amount;
        Save();
        //UICahge event
        //...
    }
    public void UpdateEasyTriggerBooster(int amount)
    {
        _easyTriggerBooster += amount;
        Save();
        //UICahge event
        //...
    }
    public void UpdateFistAidKitBooster(int amount)
    {
        _fistAidKitBooster += amount;
        Save();
        //UICahge event
        //...
    }
    public void UpdateStarshipModel(string element)
    {
        _starshipModel = element;
        Save();
        //UICahge event
        //...
    }
    public void UpdateStarshipColors(string element)
    {
        _starshipColors.Add(element);
        Save();
        //UICahge event
        //...
    }

    private static string kSavePath = Application.persistentDataPath + "/gameProgression.json";
    public void Initialize(GameConfigService config)
    {
        Load(config);
    }
    public void Save()
    {
        System.IO.File.WriteAllText(kSavePath, JsonUtility.ToJson(this));
    }

    private void Load(GameConfigService config)
    {
        if (System.IO.File.Exists(kSavePath))
        {
            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(kSavePath), this);
            return;
        }

        _alianceCredits = config.PlayerInitialAlianceCredits;
        _deAthomizerBooster = config.PlayerInitialDeAthomizerBooster;
        _dilithium = config.PlayerInitialDilithium;
        _easyTriggerBooster = config.PlayerInitialEasyTriggerBooster;
        _fistAidKitBooster = config.PlayerInitialFistAidKitBooster;
        _starshipModel = config.PlayerInitialStarshipModel;
        _starshipColors = config.PlayerInitialStarshipColors;

        Save();
    }

    public void Clear() { }
}
