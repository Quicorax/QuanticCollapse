using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableAssetPreWarm
{
    public async Task PreWarmElements(Action updateProgress)
    {
        await PreWarmUiAddressableElements(updateProgress);
        await PreWarmGameObjectsAddressableElements(updateProgress);
    }
    async Task  PreWarmUiAddressableElements(Action updateProgress)
    {
        await Addressables.LoadAssetAsync<GameObject>(Constants.ModularPopUp).Task;
        updateProgress();

        PopUpComponentData[] Modules = new PopUpComponentData[]
        {
            new HeaderPopUpComponentData(),
            new TextPopUpComponentData(),
            new ImagePopUpComponentData(Constants.AllianceCredits),
            new PricePopUpComponentData(),
            new ButtonPopUpComponentData(),
            new CloseButtonPopUpComponentData(),
        };

        foreach (var moduleData in Modules)
        {
            await Addressables.LoadAssetAsync<GameObject>(Constants.PopUpModule + moduleData.ModuleConcept).Task;
            updateProgress();
        }
    }

    async Task  PreWarmGameObjectsAddressableElements(Action updateProgress)
    {
        string[] StarshipNames = new string[]
        {
            Constants.RookName,
            Constants.RhynoName,
        };

        foreach (var starhipName in StarshipNames)
        {
            await Addressables.LoadAssetAsync<GameObject>(Constants.StarshipTPVModel + starhipName).Task;
            updateProgress();
            await Addressables.LoadAssetAsync<GameObject>(Constants.StarshipFPVModel + starhipName).Task;
            updateProgress();
        }
    }
}
