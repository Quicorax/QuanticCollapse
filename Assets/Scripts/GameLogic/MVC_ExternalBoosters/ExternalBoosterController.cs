using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;

public class ExternalBoosterController
{
    private GameProgressionService _gameProgression;
    private GridController _gridController;
    private Action<string> _boosterUsedVisualEffects;
    private string _externalBoosterOnSight;

    public ExternalBoosterController(GameProgressionService gameProgression, GridController gridController, Action<string> boosterUsedVisualEffects)
    {
        _gameProgression = gameProgression;
        _gridController = gridController;
        _boosterUsedVisualEffects = boosterUsedVisualEffects;
    }

    public void ExecuteBooster(ExternalBoosterSourceController elementBehaviour, Transform transform)
    {
        if (_gameProgression.CheckElement(elementBehaviour.boosterName) > 0)
            elementBehaviour.Execute(_gridController, ConfirmExecution);
        else
        {
            _externalBoosterOnSight = elementBehaviour.boosterName;
            ShowRewardedAdPopUp(transform);
        }
    }
    void ShowRewardedAdPopUp(Transform transform)
    {
        List<PopUpComponentData> Modules = new()
            {
                new HeaderPopUpComponentData(_externalBoosterOnSight, true),
                new TextPopUpComponentData("Your hangar is empty! \n Watch to purchase!"),

                new ImagePopUpComponentData(_externalBoosterOnSight),
                new ImagePopUpComponentData("VideoIcon"),

                new ButtonPopUpComponentData("Watch and ad", IngamePurchaseExternalBooster, true),
                new CloseButtonPopUpComponentData(),
            };

        Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
        {
            Addressables.InstantiateAsync("Modular_PopUp", transform)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };

    }
    async void IngamePurchaseExternalBooster()
    {
        if(await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            _gameProgression.UpdateElement(_externalBoosterOnSight, ServiceLocator.GetService<GameConfigService>().ExternalBoosterPerRewardedAd);
        }

        _boosterUsedVisualEffects?.Invoke(_externalBoosterOnSight);
        _externalBoosterOnSight = null;
    }

    void ConfirmExecution(string executedBoosterName, bool positive)
    {
        if (positive)
            _gameProgression.UpdateElement(executedBoosterName, -1);
        else
            _gameProgression.UpdateElement(executedBoosterName, 1);

        _boosterUsedVisualEffects?.Invoke(executedBoosterName);
    }
}
