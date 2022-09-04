﻿
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableSaveData
{
    public Progres Progres = new();
    public Configuration Configuration = new();
}

[System.Serializable]
public class Progres
{
    public int Reputation = 0;

    public int DilithiumAmount = 20;
    public int AlianceCreditsAmount = 99;

    public int FistAidKitBoosterAmount = 5;
    public int EasyTriggerBoosterAmount = 5;
    public int DeAthomizerBoosterAmount = 5;

    public bool[] LevelsCompleted = new bool[10]; //Non Lineal

    public List<DeSeializedStarshipColors> UnlockedSkins = new();
    //public int Starship Model
}

[System.Serializable]
public class Configuration
{
    public bool IsMusicOn = true;
    public bool IsSFXOn = true;

    public DeSeializedStarshipColors EquipedStarshipColorPack;
    public int EquipedStarshipPrefabIndex = 0;
}
