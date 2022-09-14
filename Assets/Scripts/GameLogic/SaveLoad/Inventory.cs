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

    public Inventory(SerializableSaveData saveFiles, GenericEventBus elementAmountModified)
    {
        SaveFiles = saveFiles;
        _ElementAmountModified = elementAmountModified;
    }

    #region SkinColors
    public void SetEquipedStarshipColors(DeSeializedStarshipColors skin) => SaveFiles.Configuration.EquipedStarshipColorPack = skin;
    public DeSeializedStarshipColors GetEquipedStarshipColors() => SaveFiles.Configuration.EquipedStarshipColorPack;
    public void AddElementToUnlockedSkins(DeSeializedStarshipColors skin) => SaveFiles.Progres.UnlockedSkins.Add(skin);
    public bool CheckSkinIsUnlockedByName(string name)
    {
        foreach (var item in SaveFiles.Progres.UnlockedSkins)
        {
            if (item.SkinName == name)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region SkinGeo
    public void SetEquipedStarshipGeoIndex(StarshipGeoModel geo) => SaveFiles.Configuration.EquipedStarshipGeo = geo;
    public StarshipGeoModel GetEquipedStarshipGeo() => SaveFiles.Configuration.EquipedStarshipGeo;
    public void AddElementToUnlockedGeo(StarshipGeoModel geo) => SaveFiles.Progres.UnlockedGeos.Add(geo);
    public bool CheckGeoIsUnlockedByName(string name)
    {
        foreach (var item in SaveFiles.Progres.UnlockedGeos)
        {
            if (item.StarshipName == name)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Game Elements
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
            SaveFiles.Progres.DilithiumAmount -= amount;
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
            return 9999999;
    }
    #endregion
}
