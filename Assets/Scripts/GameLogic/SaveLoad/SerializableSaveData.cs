
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

    public bool[] levelesCompleted = new bool[10];

    //Unlocked Skins
    //Unlocked Starship Model
}

[System.Serializable]
public class Configuration
{
    public bool IsMusicOn = true;
    public bool IsSFXOn = true;

    //public ColorPackScriptable StarshipEquipedColors;

    public int EquipedStarshipColorsIndex;
    public int EquipedStarshipPrefabIndex;
}
