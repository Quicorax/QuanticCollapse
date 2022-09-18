using System.Collections.Generic;
using UnityEngine;

public class PopUpService : IService
{
    private AddressablesService _addressables;

    public void Initialize(AddressablesService addressables)
    {
        _addressables = addressables;

        Prewarm();
    }

    public void SpawnPopUp(List<PopUpComponentData> Modules, Transform parent)
    {
        _addressables.SpawnAddressable<ModularPopUp>(Constants.ModularPopUp, parent, x => x.GeneratePopUp(Modules));
    }

    private void Prewarm()
    {

    }
    public void Clear() { }
}