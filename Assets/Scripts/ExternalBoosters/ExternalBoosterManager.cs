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

    private FistAidExternalBooster fistAidExternalBooster;
    private EasyTriggerExternalBooster easyTriggerExternalBooster;
    private DeAthomizerExternalBooster deAthomizerExternalBooster;

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
        if (TryGetSpecificBoosterElements(ExternalBoosterKind.FistAidKit, out ExternalBoosterElements fistAidSpecificElements))
        {
            fistAidExternalBooster = new(_MasterSceneManager, fistAidSpecificElements, View);
        }

        if (TryGetSpecificBoosterElements(ExternalBoosterKind.EasyTrigger, out ExternalBoosterElements easyTriggerSpecificElements))
        {
            easyTriggerExternalBooster = new(_MasterSceneManager, easyTriggerSpecificElements, View);
        }

        if (TryGetSpecificBoosterElements(ExternalBoosterKind.DeAthomizer, out ExternalBoosterElements deAthomizerSpecificElements))
        {
            deAthomizerExternalBooster = new(_MasterSceneManager, deAthomizerSpecificElements, View);
        }
    }
    private bool TryGetSpecificBoosterElements(ExternalBoosterKind expectedKind, out ExternalBoosterElements elements)
    {
        foreach (var element in ExternalBoosterElementsHolder)
        {
            if (element.kind == expectedKind)
            {
                elements = element.elements;
                return true;
            }
        }

        elements = new();
        return false;
    }

    public void ExecuteFistAidKit() { fistAidExternalBooster.Execute(); }

    public void ExecuteEasyTrigger() { easyTriggerExternalBooster.Execute(); }

    public void ExecuteDeAtomizer()
    {
        if (_inputManager.deAthomizerBoostedInput)
        {
            _MasterSceneManager.SaveFiles.progres.deAthomizerBoosterAmount++;
            deAthomizerExternalBooster.SetCountText();
            _inputManager.deAthomizerBoostedInput = false;
            return;
        }

        deAthomizerExternalBooster.Execute();
        _inputManager.deAthomizerBoostedInput = true;
    }
}
