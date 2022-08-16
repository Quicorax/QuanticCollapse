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

    public int fistAidKidBoosterAmount;    //External Booster give Life
    public int easyTriggerBoosterAmount;   //External Booster direct damage Enemy
    public int deAthomizerBoosterAmount;   //External Booster destroy grid Block

    //public int starshipModelIndex;
    //public int starshipColorIndex;
}

[System.Serializable]
public class Configuration
{
    public bool isMusicOn = true;
    public bool isSFXOn = true;
}
