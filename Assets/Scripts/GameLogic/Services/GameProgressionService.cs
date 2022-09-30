﻿using System.Collections.Generic;
using System;
using UnityEngine;

public enum ResourcesType { FirstAidKit, EasyTrigger, DeAthomizer, Dilithium, Reputation, AllianceCredits };

[Serializable]
public class ResourceElement
{
    public ResourcesType Resource;
    public int Amount;

    public ResourceElement(ResourcesType resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }
}

[Serializable]
public class GameProgressionService : IService
{
    private SaveLoadService _saveLoadService;
    public int TicksPlayed => _ticksPlayed;
    [SerializeField] private int _ticksPlayed = 0;

    [SerializeField] private List<ResourceElement> _resources = new();

    [SerializeField] private List<string> _starshipModels = new();
    [SerializeField] private List<string> _starshipColors = new();

    [SerializeField] private bool[] _levelsCompletedByIndex = new bool[5];

    [SerializeField] private bool _sfxOff = false;
    [SerializeField] private bool _musicOff = false;

    public void Initialize(SaveLoadService saveLoadService) => _saveLoadService = saveLoadService;

    #region Resources
    public void AddElement(ResourcesType resource, int initialAmount) => _resources.Add(new ResourceElement(resource, initialAmount));
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
        UnlockStarshipModel(config.PlayerInitialStarshipModel, false);
        UnlockColorPack(config.PlayerInitialStarshipColors, false);

        PlayerPrefs.SetString("EquipedStarshipModel", config.PlayerInitialStarshipModel);
        PlayerPrefs.SetString("EquipedStarshipColors", config.PlayerInitialStarshipColors);

        AddElement(ResourcesType.AllianceCredits, config.PlayerInitialAllianceCredits);
        AddElement(ResourcesType.Dilithium, config.PlayerInitialDilithium);
        AddElement(ResourcesType.DeAthomizer, config.PlayerInitialDeAthomizerBooster);
        AddElement(ResourcesType.EasyTrigger, config.PlayerInitialEasyTriggerBooster);
        AddElement(ResourcesType.FirstAidKit, config.PlayerInitialFistAidKitBooster);
        AddElement(ResourcesType.Reputation, 0);

        _saveLoadService.Save();
    }

    public void Clear() { }
}