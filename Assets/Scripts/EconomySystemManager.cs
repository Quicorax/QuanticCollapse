using System.Collections;
using UnityEngine;

public class EconomySystemManager : MonoBehaviour
{
    private MasterSceneManager master;
    [SerializeField] private GenericEventBus _DilithiumGenerated;

    public int MaxDilithiumAmount = 5;
    public float SecondsToRegenerateDilitium = 300;

    bool generatingDilithium;

    private void Awake()
    {
        master = GetComponent<MasterSceneManager>();
    }
    private void Start()
    {
        if (!CheckDilitiumMax())
            StartCoroutine(SlowDilithiumGeneration());
    }
    public bool CheckDilitiumEmpty()
    {
        return master.runtimeSaveFiles.progres.dilithiumAmount <= 0;
    }
    public bool CheckDilitiumMax()
    {
        return master.runtimeSaveFiles.progres.dilithiumAmount >= MaxDilithiumAmount;
    }
    public void UseDilithium()
    {
        master.runtimeSaveFiles.progres.dilithiumAmount--;

        StartCoroutine(SlowDilithiumGeneration());
    }

    public void AddDilithium()
    {
        if (!CheckDilitiumMax())
        {
            master.runtimeSaveFiles.progres.dilithiumAmount++;
            _DilithiumGenerated.NotifyEvent();
        }
    }

    IEnumerator SlowDilithiumGeneration()
    {
        generatingDilithium = true;

        yield return new WaitForSecondsRealtime(SecondsToRegenerateDilitium);
        AddDilithium();
        generatingDilithium = false;

        if (!CheckDilitiumMax())
            StartCoroutine(SlowDilithiumGeneration());
    }
}
