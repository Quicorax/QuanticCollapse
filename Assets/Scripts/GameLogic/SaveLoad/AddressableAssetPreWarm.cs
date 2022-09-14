using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableAssetPreWarm
{
    const string PopUpObjectAdrsKey = "Modular_PopUp";
    const string Empty = "";
    const string AlianceCredits = "AlianceCredits";
    const string PopUpModuleAdrsKey = "Module_";
    const string StarshipModelAdrsKey = "StarshipPrefab_";
    const string StarshipFPVModelArdKey = "FPV_Starship_";


    public async Task PreWarmElements(Action updateProgress)
    {
        await PreWarmUiAddressableElements(updateProgress);
        await PreWarmGameObjectsAddressableElements(updateProgress);
    }
    async Task  PreWarmUiAddressableElements(Action updateProgress)
    {
        await Addressables.LoadAssetAsync<GameObject>(PopUpObjectAdrsKey).Task;
        updateProgress();

        PopUpComponentData[] Modules = new PopUpComponentData[]
        {
            new HeaderPopUpComponentData(Empty, false),
            new TextPopUpComponentData(Empty),
            new ImagePopUpComponentData(AlianceCredits, Empty),
            new ImagePopUpComponentData(AlianceCredits, Empty),
            new ImagePopUpComponentData(AlianceCredits, Empty),
            new PricePopUpComponentData(Empty),
            new ButtonPopUpComponentData(Empty, null, true),
            new CloseButtonPopUpComponentData(),
        };

        foreach (var moduleData in Modules)
        {
            await Addressables.LoadAssetAsync<GameObject>(PopUpModuleAdrsKey + moduleData.ModuleConcept).Task;
            updateProgress();
        }
    }

    async Task  PreWarmGameObjectsAddressableElements(Action updateProgress)
    {
        string[] StarshipNames = new string[]
        {
            "Rook",
            "Rhyno",
        };

        foreach (var starhipName in StarshipNames)
        {
            await Addressables.LoadAssetAsync<GameObject>(StarshipModelAdrsKey + starhipName).Task;
            updateProgress();
            await Addressables.LoadAssetAsync<GameObject>(StarshipFPVModelArdKey + starhipName).Task;
            updateProgress();
        }
    }
}
