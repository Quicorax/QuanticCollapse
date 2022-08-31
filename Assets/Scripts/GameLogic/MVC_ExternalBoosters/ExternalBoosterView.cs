using System.Collections.Generic;
using UnityEngine;

public class ExternalBoosterView : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private ExternalBoosterScreenEffectEventBus ScreenEffects;

    [SerializeField] private GridView gridView;
    [SerializeField] private ExternalBoosterElementView _externalBoosterElementView;
    [SerializeField] private Transform _parent;
    private MasterSceneManager _masterSceneManager;

    public ExternalBoosterController Controller;

    public List<ExternalBoosterSourceController> ExternalBooster = new();

    [HideInInspector] public List<ExternalBoosterElementView> ActiveExternalBoosters = new();

    void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }

    public void SetMasterReference(MasterSceneManager masterReference) => _masterSceneManager = masterReference;
    public void Initialize()
    {
        Controller = new(_masterSceneManager, gridView.Controller, BoosterUsedVisualEffects);

        foreach (ExternalBoosterSourceController boosterElementsLogic in ExternalBooster)
        {
            ExternalBoosterElementView activeExternalBooster = Instantiate(_externalBoosterElementView, _parent);
            activeExternalBooster.Initialize(boosterElementsLogic, _masterSceneManager.Inventory, OnExecuteExternalBooster);

            ActiveExternalBoosters.Add(activeExternalBooster);
        }
    }
    void OnExecuteExternalBooster(ExternalBoosterSourceController boosterElement) => Controller.ExecuteBooster(boosterElement);

    void BoosterUsedVisualEffects(string externalBoosterName) 
    {
        ScreenEffects.NotifyEvent(externalBoosterName);
        ActiveExternalBoosters.Find(boosterElements => boosterElements.name == externalBoosterName).UpdateBoosterAmountText();
    }
}
