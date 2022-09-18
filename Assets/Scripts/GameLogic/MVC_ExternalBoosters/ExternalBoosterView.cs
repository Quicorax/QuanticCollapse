using System.Collections.Generic;
using UnityEngine;

public class ExternalBoosterView : MonoBehaviour
{
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
    public async void Initialize()
    {
        Controller = new(_gameProgression, _gridView.Controller, BoosterUsedVisualEffects);

        foreach (ExternalBoosterSourceController boosterElementsLogic in ExternalBooster)
        {
            var adrsInstance = await ServiceLocator.GetService<AddressablesService>()
                    .SpawnAddressable<ExternalBoosterElementView>(Constants.Booster, _parent);
            
            adrsInstance.Initialize(boosterElementsLogic, _gameProgression, OnExecuteExternalBooster);
            ActiveExternalBoosters.Add(adrsInstance);
        }
    }
    void OnExecuteExternalBooster(ExternalBoosterSourceController boosterElement) => Controller.ExecuteBooster(boosterElement, transform.parent);

    void BoosterUsedVisualEffects(string externalBoosterName) 
    {
        _ScreenEffects.NotifyEvent(externalBoosterName);
        ActiveExternalBoosters.Find(boosterElements => boosterElements.name == externalBoosterName).UpdateBoosterAmountText();
    }
}
