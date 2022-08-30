[System.Serializable]
public class SerializableSaveData
{
    public Progres progres = new();
    public Configuration configuration = new();
}

[System.Serializable]
public class Progres
{
    public int reputation = 0;

    public int lastCompletedLevelIndex = 0;

    public int dilithiumAmount = 20;
    public int alianceCreditsAmount = 99;

    public int fistAidKitBoosterAmount = 5;
    public int easyTriggerBoosterAmount = 5;
    public int deAthomizerBoosterAmount = 5;

    //Unlocked Skins
    //Unlocked Starship Model
}

[System.Serializable]
public class Configuration
{
    public bool isMusicOn = true;
    public bool isSFXOn = true;

    //Equiped Skins
    //Equiped Starship Model
}
