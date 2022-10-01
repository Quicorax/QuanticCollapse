using System.Collections.Generic;
using System;
using UnityEngine;

public enum ResourcesType { FirstAidKit, EasyTrigger, DeAthomizer, Dilithium, Reputation, AllianceCredits };

[Serializable]
public class ResourceElement
{
    public ResourcesType Resource;
    public int Amount;
}

[Serializable]
public class GameProgressionService : IService
{
    private SaveLoadService _saveLoadService;

    [SerializeField] private int _ticksPlayed  = 0;
    [SerializeField] private List<ResourceElement> _resources = new();

    [SerializeField] private List<string> _starshipModels = new();
    [SerializeField] private List<string> _starshipColors = new();

    [SerializeField] private bool[] _levelsCompletedByIndex = new bool[5];

    [SerializeField] private bool _sfxOff = false;
    [SerializeField] private bool _musicOff = false;

    public void Initialize(SaveLoadService saveLoadService) => _saveLoadService = saveLoadService;

    #region Resources
    public void UpdateElement(ResourcesType resource, int elementAmount, bool save = true)
    {
        foreach (var resourcePack in _resources)
        {
            if(resourcePack.Resource == resource)
                resourcePack.Amount += elementAmount;
        }

        _ticksPlayed++;

        if (save)
            _saveLoadService.Save();
    }
    public int CheckElement(ResourcesType resource)
    {
        int element = -1;

        foreach (var resourcePack in _resources)
        {
            if (resourcePack.Resource == resource)
            {
                element = resourcePack.Amount;
                break;
            }
        }

        return element;
    }
    #endregion

    #region Starship Visuals
    public void UnlockStarshipModel(string starshipName, int price, bool save = true)
    {
        _starshipModels.Add(starshipName);
        UpdateElement(ResourcesType.AllianceCredits, price);

        _ticksPlayed++;

        if (save)
            _saveLoadService.Save();
    }
    public void UnlockColorPack(string colorPackName, int price, bool save = true) 
    { 
        _starshipColors.Add(colorPackName); 
        UpdateElement(ResourcesType.AllianceCredits, price);

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
        _resources = config.Resources;

        UnlockStarshipModel(config.PlayerInitialStarshipModel, 0 , false);
        PlayerPrefs.SetString("EquipedStarshipModel", config.PlayerInitialStarshipModel);

        foreach (var item in config.PlayerInitialStarshipColors)
            UnlockColorPack(item.Name, 0, false);
        int rngColor = UnityEngine.Random.Range(0, config.PlayerInitialStarshipColors.Count);
        PlayerPrefs.SetString("EquipedStarshipColors", config.PlayerInitialStarshipColors[rngColor].Name);

        _saveLoadService.Save();
    }

    public void Clear() { }
}
