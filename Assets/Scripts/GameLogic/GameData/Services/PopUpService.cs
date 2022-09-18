using UnityEngine;

public class PopUpService : IService
{
    private AddressablesService _addressables;

    public void Initialize(AddressablesService addressables)
    {
        _addressables = addressables;

        PreloadModules();
    }

    public void SpawnPopUp(PopUpComponentData[] GenericModules, Transform parent)
    {
        _addressables.SpawnAddressable<ModularPopUp>(Constants.ModularPopUp, parent, x => 
        {
            x.GeneratePopUp(GenericModules);
        });
    }
    private void PreloadModules()
    {
        PopUpComponentData[] Modules = new PopUpComponentData[]
        {
            new HeaderPopUpComponentData(),
            new TextPopUpComponentData(),
            new ImagePopUpComponentData(Constants.AllianceCredits),
            new PricePopUpComponentData(),
            new ButtonPopUpComponentData(),
            new CloseButtonPopUpComponentData(),
        };
        SpawnPopUp(Modules, null);
    }

    public void Clear() { }
}