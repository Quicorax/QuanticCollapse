using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameProgressionService : IService
{
    private SaveLoadService _saveLoadService;

    [SerializeField] private int _ticksPlayed = 0;

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

    public int TicksPlayed => _ticksPlayed;
    public int AllianceCredits => _allianceCredits;
    public int Dilithium => _dilithium;
    public int Reputation => _reputation;
    public int DeAthomizerBooster => _deAthomizerBooster;
    public int EasyTriggerBooster => _easyTriggerBooster;
    public int FistAidKitBooster => _fistAidKitBooster;

    public void Initialize(SaveLoadService saveLoadService) => _saveLoadService = saveLoadService;

    #region Resources
    public void UpdateElement(string elementName, int elementAmount, bool save = true)
    {
        if (elementName == Constants.FirstAidKit)
            _fistAidKitBooster += elementAmount;
        else if (elementName == Constants.EasyTrigger)
            _easyTriggerBooster += elementAmount;
        else if (elementName == Constants.DeAthomizer)
            _deAthomizerBooster += elementAmount;
        else if (elementName == Constants.Dilithium)
            _dilithium += elementAmount;
        else if (elementName == Constants.Reputation)
            _reputation += elementAmount;
        else if (elementName == Constants.AllianceCredits)
            _allianceCredits += elementAmount;

        _ticksPlayed++;

        if (save)
            _saveLoadService.Save();
    }
    public int CheckElement(string elementName)
    {
        int element = -1;

        if (elementName == Constants.FirstAidKit)
            element = _fistAidKitBooster;
        else if (elementName == Constants.EasyTrigger)
            element = _easyTriggerBooster;
        else if (elementName == Constants.DeAthomizer)
            element = _deAthomizerBooster;
        else if (elementName == Constants.Dilithium)
            element = _dilithium;
        else if (elementName == Constants.Reputation)
            element = _reputation;
        else if (elementName == Constants.AllianceCredits)
            element = _allianceCredits;

        return element;
    }
    #endregion

    #region Starship Visuals
    public void UnlockStarshipModel(string starshipName, bool save = true)
    {
        _starshipModels.Add(starshipName);

        _ticksPlayed++;

        if (save)
            _saveLoadService.Save();
    }
    public void UnlockColorPack(string colorPackName, bool save = true) 
    { 
        _starshipColors.Add(colorPackName); 
        
        _ticksPlayed++;

        if (save)
            _saveLoadService.Save();
    }
    public bool CheckStarshipUnlockedByName(string starshipName) => _starshipModels.Contains(starshipName);
    public bool CheckColorPackUnlockedByName(string colorPackName) => _starshipColors.Contains(colorPackName);
    #endregion

    #region Level Progression
    public void SetLevelWithIndexCompleted(int index, bool save = true) 
    {
        _levelsCompletedByIndex[index] = true;

        _ticksPlayed++;

        if (save)
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

    public void LoadInitialResources(GameConfigService config)
    {
        UpdateElement(Constants.AllianceCredits, config.PlayerInitialAllianceCredits, false);
        UpdateElement(Constants.Dilithium, config.PlayerInitialDilithium, false);
        UpdateElement(Constants.DeAthomizer, config.PlayerInitialDeAthomizerBooster, false);
        UpdateElement(Constants.EasyTrigger, config.PlayerInitialEasyTriggerBooster, false);
        UpdateElement(Constants.FirstAidKit, config.PlayerInitialFistAidKitBooster, false);
        UnlockStarshipModel(config.PlayerInitialStarshipModel, false);
        UnlockColorPack(config.PlayerInitialStarshipColors, false);

        PlayerPrefs.SetString(Constants.EquipedStarshipModel, config.PlayerInitialStarshipModel);
        PlayerPrefs.SetString(Constants.EquipedStarshipColors, config.PlayerInitialStarshipColors);

        _saveLoadService.Save();
    }

    public void Clear() { }
}
