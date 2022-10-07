using System;
using UnityEngine;

public class ExternalBoosterController
{
    private GridController _gridController;
    private Action<string> _boosterUsedVisualEffects;
    private string _externalBoosterId;

    private GameProgressionService _gameProgression;
    private LocalizationService _localization;
    private PopUpService _popUps;
    public ExternalBoosterController(GameProgressionService gameProgression, GridController gridController, Action<string> boosterUsedVisualEffects)
    {
        _gameProgression = gameProgression;
        _gridController = gridController;
        _boosterUsedVisualEffects = boosterUsedVisualEffects;

        _popUps = ServiceLocator.GetService<PopUpService>();
        _localization = ServiceLocator.GetService<LocalizationService>();
    }

    public void ExecuteBooster(IExternalBooster elementBehaviour, Transform transform)
    {
        if (_gameProgression.CheckElement(elementBehaviour.BoosterId) > 0)
            elementBehaviour.Execute(_gridController, ConfirmExecution);
        else
        {
            _externalBoosterId = elementBehaviour.BoosterId;
            ShowRewardedAdPopUp(transform);
        }
    }
    void ShowRewardedAdPopUp(Transform transform)
    {
        _popUps.AddHeader(_externalBoosterId, true);
        _popUps.AddText(_localization.Localize("GAMEPLAY_BOOSTERS_EMPTY_HEADER") + "\n" + _localization.Localize("GAMEPLAY_BOOSTERS_EMPTY_BODY"));
        _popUps.AddImage(_externalBoosterId, string.Empty);
        _popUps.AddImage("VideoIcon", string.Empty);
        _popUps.AddButton(_localization.Localize("GAMEPLAY_BOOSTERS_WATCHADD_HEADER"), IngamePurchaseExternalBooster, true);
        _popUps.AddCloseButton();

        _popUps.SpawnPopUp(transform);
    }
    async void IngamePurchaseExternalBooster()
    {
        if(await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            _gameProgression.UpdateElement(_externalBoosterId, 
                ServiceLocator.GetService<GameConfigService>().VideoAddRewards.ExternalBoosters);
        }

        _boosterUsedVisualEffects?.Invoke(_externalBoosterId);
    }

    void ConfirmExecution(string externalBoosterId, bool positive)
    {
        _gameProgression.UpdateElement(externalBoosterId, positive ? -1 : 1);
        _boosterUsedVisualEffects?.Invoke(externalBoosterId);
    }
}
