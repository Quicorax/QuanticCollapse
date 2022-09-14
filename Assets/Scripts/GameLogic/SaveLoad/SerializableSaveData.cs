using System.Collections.Generic;

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

    public int DilithiumAmount = 5;
    public int AlianceCreditsAmount = 15;

    public int FistAidKitBoosterAmount = 5;
    public int EasyTriggerBoosterAmount = 5;
    public int DeAthomizerBoosterAmount = 5;

    public bool[] LevelsCompleted = new bool[99]; //Non Lineal

    public List<DeSeializedStarshipColors> UnlockedSkins = new();
    public List<StarshipGeoModel> UnlockedGeos = new();
}

[System.Serializable]
public class Configuration
{
    public bool IsMusicOn = true;
    public bool IsSFXOn = true;

    public DeSeializedStarshipColors EquipedStarshipColorPack;
    public StarshipGeoModel EquipedStarshipGeo;
}
