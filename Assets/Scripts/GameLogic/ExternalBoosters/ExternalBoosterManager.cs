using System.Collections.Generic;
using UnityEngine;

public enum ExternalBoosterKind { FistAidKit, EasyTrigger, DeAthomizer};

[System.Serializable]
public class ExternalBoosterHolder
{
    public ExternalBoosterKind kind;
    public ExternalBoosterElements elements;

    public bool CheckExpectedExternalBooster(ExternalBoosterKind expectedBoosterKind)
    {
        return expectedBoosterKind == kind;
    }
}
public class ExternalBoosterManager : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;

    [SerializeField] private VirtualGridView View;

    private MasterSceneManager _MasterSceneManager;
    [SerializeField] private UserInputManager _inputManager;

    [SerializeField] private List<ExternalBoosterHolder> ExternalBoosterElementsHolder = new();

    private Dictionary<ExternalBoosterKind, ExternalBoosterBase> externalBoosters = new();

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }

    private void Start()
    {
        SetInitialExternalBoosters();
    }

    void SetMasterReference(MasterSceneManager masterReference) { _MasterSceneManager = masterReference; }
    private void SetInitialExternalBoosters()
    {
       // foreach (var element in ExternalBoosterElementsHolder)
       // {
       //     ExternalBoosterBase externalBooster;
       //
       //     switch (element.kind)
       //     {
       //         default:
       //         case ExternalBoosterKind.FistAidKit:
       //         externalBooster = new FistAidExternalBooster(_MasterSceneManager, element.elements, View);
       //             break;
       //         case ExternalBoosterKind.EasyTrigger:
       //         externalBooster = new EasyTriggerExternalBooster(_MasterSceneManager, element.elements, View);
       //             break;
       //         case ExternalBoosterKind.DeAthomizer:
       //         externalBooster = new DeAthomizerExternalBooster(_MasterSceneManager, element.elements, View);
       //             break;
       //     }
       //     externalBoosters.Add(element.kind, externalBooster);
       // }
    }

    public void ExecuteExternalBooster(int kindIndex) 
    {
       // ExternalBoosterKind kind = (ExternalBoosterKind)kindIndex;
       // 
       // externalBoosters[kind].Execute(); 
    }
}
