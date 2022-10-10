using System.Collections.Generic;
using UnityEngine;

public class ExternalBoosterView
{
    private ExternalBoosterScreenEffectEventBus _ScreenEffects;

    private GridController _gridController;
    private Transform _parent;

    private ExternalBoosterController _controller;

    private IExternalBooster[] _externalBoosters = new IExternalBooster[]
    {
        new FirstAidKitExternalBoosterController(),
        new EasyTriggerExternalBoosterController(),
        new DeAthomizerExternalBoosterController(),
    };

    private List<ExternalBoosterElementView> _activeExternalBoosters = new();

    private GameProgressionService _gameProgression;

    public void Initialize(ExternalBoosterScreenEffectEventBus screenEffects, GridController gridController, Transform parent)
    {
        _ScreenEffects = screenEffects;
        _gridController = gridController;
        _parent = parent;

        _gameProgression = ServiceLocator.GetService<GameProgressionService>();

        _controller = new(_gameProgression, _gridController, BoosterUsedVisualEffects);

        foreach (IExternalBooster boosterElementsLogic in _externalBoosters)
        {
            ServiceLocator.GetService<AddressablesService>().SpawnAddressable<ExternalBoosterElementView>("BoostersElement", _parent, x => 
            { 
                x.Initialize(boosterElementsLogic, _gameProgression, OnExecuteExternalBooster);
                _activeExternalBoosters.Add(x);
            });
        }
    }
    void OnExecuteExternalBooster(IExternalBooster boosterElement) => _controller.ExecuteBooster(boosterElement, _parent);

    void BoosterUsedVisualEffects(string externalBoosterId) 
    {
        _ScreenEffects.NotifyEvent(externalBoosterId);
        _activeExternalBoosters.Find(boosterElements => boosterElements.SpecificBoosterLogic.BoosterId == externalBoosterId).UpdateBoosterAmountText();
    }
}
