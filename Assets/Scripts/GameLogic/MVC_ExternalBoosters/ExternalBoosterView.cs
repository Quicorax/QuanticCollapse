using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ExternalBoosterView : MonoBehaviour
{

    const string BoosterAdrsKey = "ExternalBoosterElement_ViewObject";

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private ExternalBoosterScreenEffectEventBus ScreenEffects;

    [SerializeField] private GridView gridView;
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
            Addressables.LoadAssetAsync<GameObject>(BoosterAdrsKey).Completed += handle =>
            {
                GameObject element = Addressables.InstantiateAsync(BoosterAdrsKey, _parent).Result;
                element.GetComponent<ExternalBoosterElementView>().Initialize(boosterElementsLogic, _masterSceneManager.Inventory, OnExecuteExternalBooster);

                ActiveExternalBoosters.Add(element.GetComponent<ExternalBoosterElementView>());
            };
        }
    }
    void OnExecuteExternalBooster(ExternalBoosterSourceController boosterElement) => Controller.ExecuteBooster(boosterElement);

    void BoosterUsedVisualEffects(string externalBoosterName) 
    {
        ScreenEffects.NotifyEvent(externalBoosterName);
        ActiveExternalBoosters.Find(boosterElements => boosterElements.name == externalBoosterName).UpdateBoosterAmountText();
    }
}
