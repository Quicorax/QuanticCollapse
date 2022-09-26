using System;
using UnityEngine;

public class ExternalBoosterController
{
    private GridController _gridController;
    private Action<ResourcesType> _boosterUsedVisualEffects;
    private ResourcesType _externalBoosterOnSight;

    private GameProgressionService _gameProgression;
    private LocalizationService _localization;
    private PopUpService _popUps;
    public ExternalBoosterController(GameProgressionService gameProgression, GridController gridController, Action<ResourcesType> boosterUsedVisualEffects)
    {
        _gameProgression = gameProgression;
        _gridController = gridController;
        _boosterUsedVisualEffects = boosterUsedVisualEffects;

        _popUps = ServiceLocator.GetService<PopUpService>();
        _localization = ServiceLocator.GetService<LocalizationService>();
    }

    public void ExecuteBooster(IExternalBooster elementBehaviour, Transform transform)
    {
        if (_gameProgression.CheckElement(elementBehaviour.BoosterType) > 0)
            elementBehaviour.Execute(_gridController, ConfirmExecution);
        else
        {
            _externalBoosterOnSight = elementBehaviour.BoosterType;
            ShowRewardedAdPopUp(transform);
        }
    }
    void ShowRewardedAdPopUp(Transform transform)
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData(_externalBoosterOnSight.ToString(), true),
            new TextPopUpComponentData(_localization.Localize("GAMEPLAY_BOOSTERS_EMPTY_HEADER") + "\n" + _localization.Localize("GAMEPLAY_BOOSTERS_EMPTY_BODY")),

            new ImagePopUpComponentData(_externalBoosterOnSight.ToString()),
            new ImagePopUpComponentData("VideoIcon"),

            new ButtonPopUpComponentData(_localization.Localize("GAMEPLAY_BOOSTERS_WATCHADD_HEADER"), IngamePurchaseExternalBooster, true),
            new CloseButtonPopUpComponentData(),
        };

        _popUps.SpawnPopUp(Modules, transform);
    }
    async void IngamePurchaseExternalBooster()
    {
        if(await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            _gameProgression.UpdateElement(_externalBoosterOnSight, ServiceLocator.GetService<GameConfigService>().ExternalBoosterPerRewardedAd);
        }

        _boosterUsedVisualEffects?.Invoke(_externalBoosterOnSight);
    }

    void ConfirmExecution(ResourcesType executedBoosterName, bool positive)
    {
        _gameProgression.UpdateElement(executedBoosterName, positive ? -1 : 1);
        _boosterUsedVisualEffects?.Invoke(executedBoosterName);
    }
}
