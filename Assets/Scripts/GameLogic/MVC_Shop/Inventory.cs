
public class Inventory
{
    const string Dilithium = "Dilithium";
    const string Reputation = "Reputation";
    const string AlianceCredits = "AlianceCredits";
    const string FirstAidKit = "FirstAidKit";
    const string EasyTrigger = "EasyTrigger";
    const string DeAthomizer = "DeAthomizer";


    private GenericEventBus _ElementAmountModified;

    public SerializableSaveData SaveFiles;

    //public int MaxDilithiumAmount = 5;
    //public float SecondsToRegenerateDilitium = 300;
    //bool generatingDilithium;

    public Inventory(SerializableSaveData saveFiles, GenericEventBus elementAmountModified)
    {
        SaveFiles = saveFiles;
        _ElementAmountModified = elementAmountModified;
    }

    //private void Start()
    //{
    //    if (CheckElementAmount(Dilithium) < MaxDilithiumAmount)
    //        StartCoroutine(SlowDilithiumGeneration());
    //}
    //IEnumerator SlowDilithiumGeneration()
    //{
    //    generatingDilithium = true;
    //    yield return new WaitForSecondsRealtime(SecondsToRegenerateDilitium);
    //    AddElement(Dilithium, 1);
    //    generatingDilithium = false;
    //
    //    if (CheckElementAmount(Dilithium) < MaxDilithiumAmount)
    //        StartCoroutine(SlowDilithiumGeneration());
    //}

    //public void SetStarshipColors(ColorPackScriptable colorPack)
    //{
    //    SaveFiles.Configuration.StarshipEquipedColors = colorPack;
    //}
    //public ColorPackScriptable GetStarshipColors()
    //{
    //    return SaveFiles.Configuration.StarshipEquipedColors;
    //}

    public void AddElement(string elementKind, int amount)
    {
        if(elementKind == FirstAidKit)
            SaveFiles.Progres.FistAidKitBoosterAmount += amount;
        else if (elementKind == EasyTrigger)
            SaveFiles.Progres.EasyTriggerBoosterAmount += amount;
        else if (elementKind == DeAthomizer)
            SaveFiles.Progres.DeAthomizerBoosterAmount += amount;
        else if (elementKind == Dilithium)
            SaveFiles.Progres.DilithiumAmount += amount;
        else if (elementKind == Reputation)
            SaveFiles.Progres.Reputation += amount;
        else if (elementKind == AlianceCredits)
            SaveFiles.Progres.AlianceCreditsAmount += amount;

        _ElementAmountModified.NotifyEvent();
    }

    public void RemoveElement(string elementKind, int amount)
    {
        if (elementKind == FirstAidKit)
            SaveFiles.Progres.FistAidKitBoosterAmount -= amount;
        else if (elementKind == EasyTrigger)
            SaveFiles.Progres.EasyTriggerBoosterAmount -= amount;
        else if (elementKind == DeAthomizer)
            SaveFiles.Progres.DeAthomizerBoosterAmount -= amount;
        else if (elementKind == Dilithium)
        {
            SaveFiles.Progres.DilithiumAmount -= amount;
            //if(!generatingDilithium)
            //    StartCoroutine(SlowDilithiumGeneration());
        }
        else if (elementKind == Reputation)
            SaveFiles.Progres.Reputation -= amount;
        else if (elementKind == AlianceCredits)
            SaveFiles.Progres.AlianceCreditsAmount -= amount;

        _ElementAmountModified.NotifyEvent();
    }
    public int CheckElementAmount(string elementKind)
    {
        if (elementKind == FirstAidKit)
            return SaveFiles.Progres.FistAidKitBoosterAmount;
        else if (elementKind == EasyTrigger)
            return SaveFiles.Progres.EasyTriggerBoosterAmount;
        else if (elementKind == DeAthomizer)
            return SaveFiles.Progres.DeAthomizerBoosterAmount;
        else if (elementKind == Dilithium)
            return SaveFiles.Progres.DilithiumAmount;
        else if (elementKind == Reputation)
            return SaveFiles.Progres.Reputation;
        else if (elementKind == AlianceCredits)
            return SaveFiles.Progres.AlianceCreditsAmount;
        else
            return 123456789;
    }
}
