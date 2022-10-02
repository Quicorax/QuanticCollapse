using System.Collections.Generic;
using UnityEngine;

public class ExternalBoosterView : MonoBehaviour
{
    [SerializeField] private ExternalBoosterScreenEffectEventBus _ScreenEffects;

    [SerializeField] private GridView _gridView;
    [SerializeField] private Transform _parent;

    public ExternalBoosterController Controller;

    public List<ExternalBooster> ExternalBoosters = new();

    [HideInInspector] public List<ExternalBoosterElementView> ActiveExternalBoosters = new();

    private GameProgressionService _gameProgression;
    private AddressablesService _addressables;

    void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        _addressables = ServiceLocator.GetService<AddressablesService>();
    }
    public void Initialize()
    {
        Controller = new(_gameProgression, _gridView.Controller, BoosterUsedVisualEffects);

        foreach (IExternalBooster boosterElementsLogic in ExternalBoosters)
        {
            _addressables.SpawnAddressable<ExternalBoosterElementView>("BoostersElement", _parent, x => 
            { 
                x.Initialize(boosterElementsLogic, _gameProgression, OnExecuteExternalBooster);
                ActiveExternalBoosters.Add(x);
            });
        }
    }
    void OnExecuteExternalBooster(IExternalBooster boosterElement) => Controller.ExecuteBooster(boosterElement, transform.parent);

    void BoosterUsedVisualEffects(ResourcesType externalBoosterName) 
    {
        _ScreenEffects.NotifyEvent(externalBoosterName);
        ActiveExternalBoosters.Find(boosterElements => boosterElements.SpecificBoosterLogic.BoosterType == externalBoosterName).UpdateBoosterAmountText();
    }
}
