using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ExternalBoosterView : MonoBehaviour
{

    const string BoosterAdrsKey = "ExternalBoosterElement_ViewObject";

    [SerializeField] private ExternalBoosterScreenEffectEventBus _ScreenEffects;

    [SerializeField] private GridView _gridView;
    [SerializeField] private Transform _parent;

    public ExternalBoosterController Controller;

    public List<ExternalBoosterSourceController> ExternalBooster = new();

    [HideInInspector] public List<ExternalBoosterElementView> ActiveExternalBoosters = new();

    private GameProgressionService _gameProgression;

    void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }
    public void Initialize()
    {
        Controller = new(_gameProgression, _gridView.Controller, BoosterUsedVisualEffects);

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
    void OnExecuteExternalBooster(ExternalBoosterSourceController boosterElement) => Controller.ExecuteBooster(boosterElement, transform.parent);

    void BoosterUsedVisualEffects(string externalBoosterName) 
    {
        _ScreenEffects.NotifyEvent(externalBoosterName);
        ActiveExternalBoosters.Find(boosterElements => boosterElements.name == externalBoosterName).UpdateBoosterAmountText();
    }
}
