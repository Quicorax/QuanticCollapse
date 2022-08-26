using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GenericEventBus _ElementAmountModified;

    public SerializableSaveData SaveFiles;

    public int MaxDilithiumAmount = 5;
    public float SecondsToRegenerateDilitium = 300;
    bool generatingDilithium;
    private void Awake()
    {
        SaveFiles = GetComponent<MasterSceneManager>().SaveFiles;
    }
    private void Start()
    {
        if (CheckElementAmount("Dilithium") < MaxDilithiumAmount)
            StartCoroutine(SlowDilithiumGeneration());
    }
    IEnumerator SlowDilithiumGeneration()
    {
        generatingDilithium = true;
        yield return new WaitForSecondsRealtime(SecondsToRegenerateDilitium);
        AddElement("Dilithium", 1);
        generatingDilithium = false;

        if (CheckElementAmount("Dilithium") < MaxDilithiumAmount)
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

        _ElementAmountModified.NotifyEvent();
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
        {
            SaveFiles.progres.dilithiumAmount -= amount;
            if(!generatingDilithium)
                StartCoroutine(SlowDilithiumGeneration());
        }
        else if (elementKind == "Reputation")
            SaveFiles.progres.reputation -= amount;
        else if (elementKind == "AlianceCredits")
            SaveFiles.progres.alianceCreditsAmount -= amount;

        _ElementAmountModified.NotifyEvent();
    }
    public int CheckElementAmount(string elementKind)
    {
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
            return 123456789;
    }
}
