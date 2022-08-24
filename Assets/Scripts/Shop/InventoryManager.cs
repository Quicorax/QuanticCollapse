using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GenericEventBus _ElementAmountModified;

    private SerializableSaveData SaveFiles;

    public int MaxDilithiumAmount = 5;
    public float SecondsToRegenerateDilitium = 300;

    private void Awake()
    {
        SaveFiles = GetComponent<MasterSceneManager>().SaveFiles;
    }
    private void Start()
    {
        if (!CheckDilitiumMax())
            StartCoroutine(SlowDilithiumGeneration());
    }
    public bool CheckDilitiumEmpty() { return SaveFiles.progres.dilithiumAmount <= 0; } 
    public bool CheckDilitiumMax() { return SaveFiles.progres.dilithiumAmount >= MaxDilithiumAmount; }
    public void UseDilithium()
    {
        SaveFiles.progres.dilithiumAmount--;
        StartCoroutine(SlowDilithiumGeneration());
    }

    public void AddDilithium()
    {
        if (!CheckDilitiumMax())
        {
            SaveFiles.progres.dilithiumAmount++;
            _ElementAmountModified.NotifyEvent();
        }
    }
    IEnumerator SlowDilithiumGeneration()
    {
        yield return new WaitForSecondsRealtime(SecondsToRegenerateDilitium);
        AddDilithium();

        if (!CheckDilitiumMax())
            StartCoroutine(SlowDilithiumGeneration());
    }

    public void AddElement(string elementKind, int amount)
    {
        if(elementKind == "FirstAidKit")
            SaveFiles.progres.fistAidKitBoosterAmount += amount;
        else if (elementKind == "EasyTrigger")
            SaveFiles.progres.easyTriggerBoosterAmount += amount;
        else if (elementKind == "DeAthomizer")
            SaveFiles.progres.deAthomizerBoosterAmount += amount;
        else if (elementKind == "Dilithium")
            SaveFiles.progres.dilithiumAmount += amount;
        else if (elementKind == "Reputation")
            SaveFiles.progres.reputation += amount;
        else if (elementKind == "AlianceCredits")
            SaveFiles.progres.alianceCreditsAmount += amount;
    }

    public void RemoveElement(string elementKind, int amount)
    {
        if (elementKind == "FirstAidKit")
            SaveFiles.progres.fistAidKitBoosterAmount -= amount;
        else if (elementKind == "EasyTrigger")
            SaveFiles.progres.easyTriggerBoosterAmount -= amount;
        else if (elementKind == "DeAthomizer")
            SaveFiles.progres.deAthomizerBoosterAmount -= amount;
        else if (elementKind == "Dilithium")
            SaveFiles.progres.dilithiumAmount -= amount;
        else if (elementKind == "Reputation")
            SaveFiles.progres.reputation -= amount;
        else if (elementKind == "AlianceCredits")
            SaveFiles.progres.alianceCreditsAmount -= amount;
    }
    public int CheckElementAmount(string elementKind)
    {
        //GetElementAmout(elementKind, out int elementAmount);
        //return elementAmount;

        if (elementKind == "FirstAidKit")
            return SaveFiles.progres.fistAidKitBoosterAmount;
        else if (elementKind == "EasyTrigger")
            return SaveFiles.progres.easyTriggerBoosterAmount;
        else if (elementKind == "DeAthomizer")
            return SaveFiles.progres.deAthomizerBoosterAmount;
        else if (elementKind == "Dilithium")
            return SaveFiles.progres.dilithiumAmount;
        else if (elementKind == "Reputation")
            return SaveFiles.progres.reputation;
        else if (elementKind == "AlianceCredits")
            return SaveFiles.progres.alianceCreditsAmount;
        else
            return 404;
    }

    //public void GetElementAmout(string elementKind, out int elementAmountRex)
    //{
    //    if (elementKind == "FirstAidKit")
    //        elementAmountRex = SaveFiles.progres.fistAidKitBoosterAmount;
    //    else if (elementKind == "EasyTrigger")
    //        elementAmountRex = SaveFiles.progres.easyTriggerBoosterAmount;
    //    else if (elementKind == "DeAthomizer")
    //        elementAmountRex = SaveFiles.progres.deAthomizerBoosterAmount;
    //    else if (elementKind == "Dilithium")
    //        elementAmountRex = SaveFiles.progres.dilithiumAmount;
    //    else if (elementKind == "Reputation")
    //        elementAmountRex = SaveFiles.progres.reputation;
    //    else //if (elementKind == "AlianceCredits")
    //        elementAmountRex = SaveFiles.progres.alianceCreditsAmount;
    //}
}