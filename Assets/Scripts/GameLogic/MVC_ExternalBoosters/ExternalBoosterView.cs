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
    private MasterSceneTransitioner _masterSceneManager;

    public ExternalBoosterController Controller;

    public List<ExternalBoosterSourceController> ExternalBooster = new();

    [HideInInspector] public List<ExternalBoosterElementView> ActiveExternalBoosters = new();

    private GameProgressionService _gameProgression;
    void Awake()
    {
        _MasterReference.Event += SetMasterReference;
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }

    public void SetMasterReference(MasterSceneTransitioner masterReference) => _masterSceneManager = masterReference;
    public void Initialize()
    {
        Controller = new(_gameProgression, gridView.Controller, BoosterUsedVisualEffects);

        foreach (ExternalBoosterSourceController boosterElementsLogic in ExternalBooster)
        {
            Addressables.LoadAssetAsync<GameObject>(BoosterAdrsKey).Completed += handle =>
            {
                GameObject element = Addressables.InstantiateAsync(BoosterAdrsKey, _parent).Result;
                element.GetComponent<ExternalBoosterElementView>().Initialize(boosterElementsLogic, _gameProgression, OnExecuteExternalBooster);

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
