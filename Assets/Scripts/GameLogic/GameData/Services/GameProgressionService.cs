using System.Collections.Generic;
using UnityEngine;

public enum ResourcesType { FirstAidKit, EasyTrigger, DeAthomizer, Dilithium, Reputation, AllianceCredits };
//
//[System.Serializable]
//public class ResourceElement
//{
//    public ResourcesType Resource;
//    public int Amount;
//}

[System.Serializable]
public class GameProgressionService : IService
{
    private SaveLoadService _saveLoadService;

    [SerializeField] private int _ticksPlayed = 0;

    //[SerializeField] private List<ResourceElement> _resources = new();

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
    public void UpdateElement(ResourcesType resource, int elementAmount, bool save = true)
    {
        switch (resource)
        {
            case ResourcesType.FirstAidKit:
                _fistAidKitBooster += elementAmount;
                break;
            case ResourcesType.EasyTrigger:
                _easyTriggerBooster += elementAmount;
                break;
            case ResourcesType.DeAthomizer:
                _deAthomizerBooster += elementAmount;
                break;
            case ResourcesType.Dilithium:
                _dilithium += elementAmount;
                break;
            case ResourcesType.Reputation:
                _reputation += elementAmount;
                break;
            case ResourcesType.AllianceCredits:
                _allianceCredits += elementAmount;
                break;
        }

        _ticksPlayed++;

        if (save)
            _saveLoadService.Save();
    }
    public int CheckElement(ResourcesType resource)
    {
        int element = -1;
        switch (resource)
        {
            case ResourcesType.FirstAidKit:
                element = _fistAidKitBooster;
                break;
            case ResourcesType.EasyTrigger:
                element = _easyTriggerBooster;
                break;
            case ResourcesType.DeAthomizer:
                element = _deAthomizerBooster;
                break;
            case ResourcesType.Dilithium:
                element = _dilithium;
                break;
            case ResourcesType.Reputation:
                element = _reputation;
                break;
            case ResourcesType.AllianceCredits:
                element = _allianceCredits;
                break;
        }

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
        UpdateElement(ResourcesType.AllianceCredits, config.PlayerInitialAllianceCredits, false);
        UpdateElement(ResourcesType.Dilithium, config.PlayerInitialDilithium, false);
        UpdateElement(ResourcesType.DeAthomizer, config.PlayerInitialDeAthomizerBooster, false);
        UpdateElement(ResourcesType.EasyTrigger, config.PlayerInitialEasyTriggerBooster, false);
        UpdateElement(ResourcesType.FirstAidKit, config.PlayerInitialFistAidKitBooster, false);

        UnlockStarshipModel(config.PlayerInitialStarshipModel, false);
        UnlockColorPack(config.PlayerInitialStarshipColors, false);

        PlayerPrefs.SetString("EquipedStarshipModel", config.PlayerInitialStarshipModel);
        PlayerPrefs.SetString("EquipedStarshipColors", config.PlayerInitialStarshipColors);

        _saveLoadService.Save();
    }

    public void Clear() { }
}
