using System.Collections;
using UnityEngine;

public class EconomySystemManager : MonoBehaviour
{
    private MasterSceneManager _MasterSceneManager;
    [SerializeField] private GenericEventBus _DilithiumGenerated;

    public int MaxDilithiumAmount = 5;
    public float SecondsToRegenerateDilitium = 300;

    private void Awake()
    {
        _MasterSceneManager = GetComponent<MasterSceneManager>();
    }
    private void Start()
    {
        if (!CheckDilitiumMax())
            StartCoroutine(SlowDilithiumGeneration());
    }
    public bool CheckDilitiumEmpty()
    {
        return _MasterSceneManager.runtimeSaveFiles.progres.dilithiumAmount <= 0;
    }
    public bool CheckDilitiumMax()
    {
        return _MasterSceneManager.runtimeSaveFiles.progres.dilithiumAmount >= MaxDilithiumAmount;
    }
    public void UseDilithium()
    {
        _MasterSceneManager.runtimeSaveFiles.progres.dilithiumAmount--;

        StartCoroutine(SlowDilithiumGeneration());
    }

    public void AddDilithium()
    {
        if (!CheckDilitiumMax())
        {
            _MasterSceneManager.runtimeSaveFiles.progres.dilithiumAmount++;
            _DilithiumGenerated.NotifyEvent();
        }
    }

    IEnumerator SlowDilithiumGeneration()
    {
        yield return new WaitForSecondsRealtime(SecondsToRegenerateDilitium);
        AddDilithium();

        if (!CheckDilitiumMax())
            StartCoroutine(SlowDilithiumGeneration());
    }
}
