using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameProgressionService : IService
{
    const string DilithiumName = "Dilithium";
    const string ReputationName = "Reputation";
    const string AllianceCreditsName = "AllianceCredits";
    const string FirstAidKitName = "FirstAidKit";
    const string EasyTriggerName = "EasyTrigger";
    const string DeAthomizerName = "DeAthomizer";

    private SaveLoadService _saveLoadService;

    [SerializeField] private int _allianceCredits = 0;
    [SerializeField] private int _dilithium = 0;
    [SerializeField] private int _reputation = 0;
    [SerializeField] private int _deAthomizerBooster = 0;
    [SerializeField] private int _easyTriggerBooster = 0;
    [SerializeField] private int _fistAidKitBooster = 0;

    [SerializeField] private List<string> _starshipModels = new();
    [SerializeField] private List<string> _starshipColors = new();

    [SerializeField] private bool[] _levelsCompletedByIndex = new bool[5];

    [SerializeField] private bool _sfxOff = false;
    [SerializeField] private bool _musicOff = false;

    public int AllianceCredits => _allianceCredits;
    public int Dilithium => _dilithium;
    public int Reputation => _reputation;
    public int DeAthomizerBooster => _deAthomizerBooster;
    public int EasyTriggerBooster => _easyTriggerBooster;
    public int FistAidKitBooster => _fistAidKitBooster;

    public List<string> StarshipModels => _starshipModels;
    public List<string> StarshipColors => _starshipColors;
    public bool[] LevelsCompletedByIndex => _levelsCompletedByIndex;


    public void Initialize(SaveLoadService saveLoadService) => _saveLoadService = saveLoadService;

    #region Resources
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
        else if (elementName == AllianceCreditsName)
            _allianceCredits += elementAmount;

        _saveLoadService.Save();
    }
    public int CheckElement(string elementName)
    {
        int element = -1;

        if (elementName == FirstAidKitName)
            element = _fistAidKitBooster;
        else if (elementName == EasyTriggerName)
            element = _easyTriggerBooster;
        else if (elementName == DeAthomizerName)
            element = _deAthomizerBooster;
        else if (elementName == DilithiumName)
            element = _dilithium;
        else if (elementName == ReputationName)
            element = _reputation;
        else if (elementName == AllianceCreditsName)
            element = _allianceCredits;

        return element;
    }
    #endregion

    #region Starship Visuals
    public void UnlockStarshipModel(string starshipName)
    {
        _starshipModels.Add(starshipName);
        _saveLoadService.Save();
    }
    public void UnlockColorPack(string colorPackName) 
    { 
        _starshipColors.Add(colorPackName);
        _saveLoadService.Save();
    }
    public bool CheckStarshipUnlockedByName(string starshipName) => _starshipModels.Contains(starshipName);
    public bool CheckColorPackUnlockedByName(string colorPackName) => _starshipColors.Contains(colorPackName);
    #endregion

    #region Level Progression
    public void SetLevelWithIndexCompleted(int index) 
    {
        _levelsCompletedByIndex[index] = true;
        _saveLoadService.Save();
    }
    public bool CheckLevelWithIndexIsCompleted(int index) => _levelsCompletedByIndex[index];
    #endregion

    #region Audio Settings
    public void SetSFXOff(bool off) 
    { 
        _sfxOff = off;
        _saveLoadService.Save();
    }
    public void SetMusicOff(bool off)
    {
        _musicOff = off;
        _saveLoadService.Save();
    } 
    public bool CheckSFXOff() => _sfxOff;
    public bool CheckMusicOff() => _musicOff;
    #endregion

    public void Clear() { }
}
