[System.Serializable]
public class SerializableSaveData
{
    public Progres progres;
    public Configuration configuration;
}

[System.Serializable]
public class Progres
{
    public int lastCompletedLevelIndex;

    public int dilithiumAmount;
    public int alianceCreditsAmount;

    public int fistAidKidBoosterAmount;
    public int easyTriggerBoosterAmount;
    public int deAthomizerBoosterAmount;

    //public int starshipModelIndex;
    //public int starshipColorIndex;
}

[System.Serializable]
public class Configuration
{
    public bool isMusicOn = true;
    public bool isSFXOn = true;
}
