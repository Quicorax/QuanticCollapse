using System;
using UnityEngine;

namespace QuanticCollapse
{
    public class ExternalBoosterController
    {
        private readonly GridModel _gridModel;

        private readonly GameProgressionService _gameProgression;
        private readonly LocalizationService _localization;
        private readonly PopUpService _popUps;

        private readonly Action<string> _boosterUsedVisualEffects;

        private string _externalBoosterId;

        public ExternalBoosterController(
            GameProgressionService gameProgression,
            GridModel gridModel,
            Action<string> boosterUsedVisualEffects)
        {
            _gameProgression = gameProgression;
            _gridModel = gridModel;
            _boosterUsedVisualEffects = boosterUsedVisualEffects;

            _popUps = ServiceLocator.GetService<PopUpService>();
            _localization = ServiceLocator.GetService<LocalizationService>();
        }

        public void ExecuteBooster(IExternalBooster elementBehaviour, Transform transform)
        {
            if (_gameProgression.CheckElement(elementBehaviour.BoosterId) > 0)
                elementBehaviour.Execute(_gridModel, ConfirmExecution);
            else
            {
                _externalBoosterId = elementBehaviour.BoosterId;
                ShowRewardedAdPopUp(transform);
            }
        }

        private async void InGamePurchaseExternalBooster()
        {
            if (await ServiceLocator.GetService<AdsGameService>().ShowAd())
            {
                _gameProgression.UpdateElement(_externalBoosterId,
                    ServiceLocator.GetService<GameConfigService>().VideoAddRewards.ExternalBoosters);
            }

            _boosterUsedVisualEffects?.Invoke(_externalBoosterId);
        }

        private void ConfirmExecution(string externalBoosterId, bool positive)
        {
            _gameProgression.UpdateElement(externalBoosterId, positive ? -1 : 1);
            _boosterUsedVisualEffects?.Invoke(externalBoosterId);
        }

        private void ShowRewardedAdPopUp(Transform transform)
        {
            _popUps.SpawnPopUp(transform.parent, new IPopUpComponentData[]
            {
                _popUps.AddHeader(_externalBoosterId, true),
                _popUps.AddText(_localization.Localize("GAMEPLAY_BOOSTERS_EMPTY_HEADER") + "\n" +
                                _localization.Localize("GAMEPLAY_BOOSTERS_EMPTY_BODY")),
                _popUps.AddImage(_externalBoosterId, string.Empty),
                _popUps.AddImage("VideoIcon", string.Empty),
                _popUps.AddButton(_localization.Localize("GAMEPLAY_BOOSTERS_WATCHADD_HEADER"),
                    InGamePurchaseExternalBooster, true),
                _popUps.AddCloseButton()
            });
        }
    }
}