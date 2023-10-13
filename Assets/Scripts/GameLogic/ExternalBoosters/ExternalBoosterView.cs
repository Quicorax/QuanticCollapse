using System.Collections.Generic;
using UnityEngine;

namespace QuanticCollapse
{
    public class ExternalBoosterView
    {
        private ExternalBoosterScreenEffectEventBus _screenEffects;
        private GameProgressionService _gameProgression;
        private ExternalBoosterController _controller;

        private GridModel _gridModel;
        private Transform _parent;

        private readonly List<ExternalBoosterElementView> _activeExternalBoosters = new();

        private readonly IExternalBooster[] _externalBoosters =
        {
            new FirstAidKitExternalBoosterController(),
            new EasyTriggerExternalBoosterController(),
            new DeAthomizerExternalBoosterController(),
        };

        public void Initialize(ExternalBoosterScreenEffectEventBus screenEffects, GridModel model, Transform parent)
        {
            _screenEffects = screenEffects;
            _gridModel = model;
            _parent = parent;

            _gameProgression = ServiceLocator.GetService<GameProgressionService>();

            _controller = new(_gameProgression, _gridModel, BoosterUsedVisualEffects);

            foreach (var boosterElementsLogic in _externalBoosters)
            {
                ServiceLocator.GetService<AddressablesService>().LoadAddrsOfComponent<ExternalBoosterElementView>(
                    "BoostersElement", _parent, view =>
                    {
                        view.Initialize(boosterElementsLogic, _gameProgression, OnExecuteExternalBooster);
                        _activeExternalBoosters.Add(view);
                    });
            }
        }

        private void OnExecuteExternalBooster(IExternalBooster boosterElement) =>
            _controller.ExecuteBooster(boosterElement, _parent);

        private void BoosterUsedVisualEffects(string externalBoosterId)
        {
            _screenEffects.NotifyEvent(externalBoosterId);
            _activeExternalBoosters
                .Find(boosterElements => boosterElements.SpecificBoosterLogic.BoosterId == externalBoosterId)
                .UpdateBoosterAmountText();
        }
    }
}