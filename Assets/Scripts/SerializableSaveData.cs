﻿[System.Serializable]
public class SerializableSaveData
{
    public Progres progres;
    public Configuration configuration;

    public SerializableSaveData()
    {
        this.progres = new();
        this.configuration = new();
    }
}

[System.Serializable]
public class Progres
{
    public int reputation = 0;

    public int lastCompletedLevelIndex = 0;

    public int dilithiumAmount = 20;
    public int alianceCreditsAmount = 99;

    public int fistAidKidBoosterAmount = 5;
    public int easyTriggerBoosterAmount = 5;
    public int deAthomizerBoosterAmount = 5;

    //public int starshipModelIndex;
    //public int starshipColorIndex;
}

[System.Serializable]
public class Configuration
{
    public bool isMusicOn = true;
    public bool isSFXOn = true;
}
