using System.Collections.Generic;
using UnityEngine;

public class ExternalBoosterView : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private ExternalBoosterScreenEffectEventBus ScreenEffects;

    [SerializeField] private VirtualGridView gridView;
    [SerializeField] private ExternalBoosterElementView _externalBoosterElementView;
    [SerializeField] private Transform _parent;
    private MasterSceneManager _masterSceneManager;

    public ExternalBoosterController Controller;

    public List<ExternalBoosterBase> ExternalBooster = new List<ExternalBoosterBase>();

    [HideInInspector] public List<ExternalBoosterElementView> ActiveExternalBoosters = new();

    void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    private void Start()
    {
        Initialize();
    }

    public void SetMasterReference(MasterSceneManager masterReference) => _masterSceneManager = masterReference;
    public void Initialize()
    {
        Controller = new(_masterSceneManager, gridView, BoosterUsedEffects);

        foreach (ExternalBoosterBase boosterElements in ExternalBooster)
        {
            ExternalBoosterElementView activeExternalBooster = Instantiate(_externalBoosterElementView, _parent);
            activeExternalBooster.Initialize(boosterElements, _masterSceneManager.Inventory, OnExecuteExternalBooster);

            ActiveExternalBoosters.Add(activeExternalBooster);
        }
    }
    void OnExecuteExternalBooster(ExternalBoosterBase boosterElement)
    {
        if(_masterSceneManager.Inventory.CheckElementAmount(boosterElement.boosterName) > 0)
            Controller.ExecuteBooster(boosterElement);
    }

    public void BoosterUsedEffects(string externalBoosterName) 
    {
        ScreenEffects.NotifyEvent(externalBoosterName);
        ActiveExternalBoosters.Find(boosterElements => boosterElements.name == externalBoosterName).ExternalBoosterUsedEffect();
    }
}
