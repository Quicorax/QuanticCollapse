using UnityEngine;

public class PopUpService : IService
{
    private AddressablesService _addressables;

    public void Initialize(AddressablesService addressables)
    {
        _addressables = addressables;

        PreloadModules();
    }

    public void SpawnPopUp(IPopUpComponentData[] GenericModules, Transform parent)
    {
        _addressables.SpawnAddressable<ModularPopUp>("Modular_PopUp", parent, x => 
        {
            x.GeneratePopUp(GenericModules);
        });
    }
    private void PreloadModules()
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData(""),
            new TextPopUpComponentData(""),
            new ImagePopUpComponentData("AllianceCredits"),
            new PricePopUpComponentData(""),
            new ButtonPopUpComponentData(),
            new CloseButtonPopUpComponentData(),
        };
        SpawnPopUp(Modules, null);
    }

    public void Clear() { }
}