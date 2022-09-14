using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameProgressionService : IService
{
    const string DilithiumName = "Dilithium";
    const string ReputationName = "Reputation";
    const string AlianceCreditsName = "AlianceCredits";
    const string FirstAidKitName = "FirstAidKit";
    const string EasyTriggerName = "EasyTrigger";
    const string DeAthomizerName = "DeAthomizer";

    [SerializeField] private int _alianceCredits;
    [SerializeField] private int _dilithium;
    [SerializeField] private int _reputation;
    [SerializeField] private int _deAthomizerBooster;
    [SerializeField] private int _easyTriggerBooster;
    [SerializeField] private int _fistAidKitBooster;
    [SerializeField] private List<string> _starshipModels = new();
    [SerializeField] private List<string> _starshipColors = new();

    [SerializeField] private bool[] _levelsCompletedByIndex = new bool[5];

    [SerializeField] private bool _sfxOff;
    [SerializeField] private bool _musicOff;

    public int AlianceCredits => _alianceCredits;
    public int Dilithium => _dilithium;
    public int Reputation => _reputation;
    public int DeAthomizerBooster => _deAthomizerBooster;
    public int EasyTriggerBooster => _easyTriggerBooster;
    public int FistAidKitBooster => _fistAidKitBooster;
    public List<string> StarshipModels => _starshipModels;
    public List<string> StarshipColors => _starshipColors;
    public bool[] LevelsCompletedByIndex => _levelsCompletedByIndex;

    public void UpdateElement(string elementName, int elementAmount)
    {
        if (elementName == FirstAidKitName)
            _fistAidKitBooster += elementAmount;
        else if (elementName == EasyTriggerName)
            _easyTriggerBooster += elementAmount;
        else if (elementName == DeAthomizerName)
            _deAthomizerBooster += elementAmount;
        else if (elementName == DilithiumName)
            _dilithium += elementAmount;
        else if (elementName == ReputationName)
            _reputation += elementAmount;
        else if (elementName == AlianceCreditsName)
            _alianceCredits += elementAmount;

        Save();
    }
    public int CheckElement(string elementName)
    {
        if (elementName == FirstAidKitName)
            return _fistAidKitBooster;
        else if (elementName == EasyTriggerName)
            return _easyTriggerBooster;
        else if (elementName == DeAthomizerName)
            return _deAthomizerBooster;
        else if (elementName == DilithiumName)
            return _dilithium;
        else if (elementName == ReputationName)
            return _reputation;
        else //if (elementName == AlianceCreditsName)
            return _alianceCredits;

        Save();
    }
    public void UnlockStarshipModel(string starshipName)
    {
        _starshipModels.Add(starshipName);
        Save();
    }
    public void UnlockColorPack(string colorPackName) 
    { 
        _starshipModels.Add(colorPackName);
        Save();
    }
    public bool CheckColorPackUnlockedByName(string colorPackName) => _starshipModels.Contains(colorPackName);
    public bool CheckStarshipUnlockedByName(string starshipName) => _starshipColors.Contains(starshipName);

    public void SetLevelWithIndexCompleted(int index) 
    {
        _levelsCompletedByIndex[index] = true;
        Save();
    }
    public bool CheckLevelWithIndexIsCompleted(int index) => _levelsCompletedByIndex[index];

    public void SetSFXOff(bool off) 
    { 
        _sfxOff = off;
        Save();
    }
    public void SetMusicOff(bool off)
    {
        _musicOff = off;
        Save();
    } 
    public bool CheckSFXOff() => _sfxOff;
    public bool CheckMusicOff() => _musicOff;


    private static string kSavePath = Application.persistentDataPath + "/_gameProgression.json";
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
        _starshipModels.Add(config.PlayerInitialStarshipModel);
        _starshipColors.Add(config.PlayerInitialStarshipColors);

        PlayerPrefs.SetString("EquipedStarshipModel", config.PlayerInitialStarshipModel);
        PlayerPrefs.SetString("EquipedStarshipColors", config.PlayerInitialStarshipColors);

        Save();
    }

    public void Clear() { }
}
