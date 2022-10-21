using System;
using UnityEngine;

namespace QuanticCollapse
{
    public class ExternalBoosterController
    {
        private GridModel _gridModel;
        private Action<string> _boosterUsedVisualEffects;
        private string _externalBoosterId;

        private GameProgressionService _gameProgression;
        private LocalizationService _localization;
        private PopUpService _popUps;
        public ExternalBoosterController(GameProgressionService gameProgression, GridModel gridModel, Action<string> boosterUsedVisualEffects)
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

        private async void IngamePurchaseExternalBooster()
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
                _popUps.AddText(_localization.Localize("GAMEPLAY_BOOSTERS_EMPTY_HEADER") + "\n"
                    + _localization.Localize("GAMEPLAY_BOOSTERS_EMPTY_BODY")),
                _popUps.AddImage(_externalBoosterId, string.Empty),
                _popUps.AddImage("VideoIcon", string.Empty),
                _popUps.AddButton(_localization.Localize("GAMEPLAY_BOOSTERS_WATCHADD_HEADER"),
                    IngamePurchaseExternalBooster, true),
                _popUps.AddCloseButton()
            });
        }
    }
}