using System.Collections.Generic;
using UnityEngine;

namespace QuanticCollapse
{
    public class ExternalBoosterView
    {
        private ExternalBoosterScreenEffectEventBus _ScreenEffects;

        private GridModel _gridModel;
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

        public void Initialize(ExternalBoosterScreenEffectEventBus screenEffects, GridModel model, Transform parent)
        {
            _ScreenEffects = screenEffects;
            _gridModel = model;
            _parent = parent;

            _gameProgression = ServiceLocator.GetService<GameProgressionService>();

            _controller = new(_gameProgression, _gridModel, BoosterUsedVisualEffects);

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
}