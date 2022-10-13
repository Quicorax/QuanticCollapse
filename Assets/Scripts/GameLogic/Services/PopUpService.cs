using System;
using System.Collections.Generic;
using UnityEngine;
public class PopUpService : IService
{
    private AddressablesService _addressables;
    public List<IPopUpComponentData> _addedModules = new();
    private ModulesPool _modulesPool = new();

    public void Initialize(AddressablesService addressables)
    {
        _addressables = addressables;
        _modulesPool.Initialize();

        PreloadModules();
    }

    private void PreloadModules()
    {
        AddHeader(string.Empty, false);
        AddText(string.Empty);
        AddImage("AllianceCredits", string.Empty);
        AddPrice(string.Empty);
        AddButton(string.Empty, null, false);
        AddCloseButton();

        SpawnPopUp(null);
    }
    public void SpawnPopUp(Transform parent) 
        => _addressables.SpawnAddressable<ModularPopUp>("Modular_PopUp", parent, x => x.GeneratePopUp(_addedModules));

    public void DeSpawnPopUp()
    {
        foreach (var item in _addedModules)
            _modulesPool.DeSpawnModule(item.ModuleConcept, item);

        _addedModules.Clear();
    }
    public void AddHeader(string text, bool cond)
    {
        var component = _modulesPool.SpawnModule(PopUpComponentType.Header) as HeaderPopUpComponentData;
        component.HeaderTextContent = text;
        component.IsHeaderHighlighted = cond;
        _addedModules.Add(component);
    }
    public void AddText(string text)
    {
        var component = _modulesPool.SpawnModule(PopUpComponentType.Text) as TextPopUpComponentData;
        component.TextContent = text;
        _addedModules.Add(component);
    }
    public void AddImage(string text, string text1)
    {
        var component = _modulesPool.SpawnModule(PopUpComponentType.Image) as ImagePopUpComponentData;
        component.SpriteName = text;
        component.ImageText = text1;
        component.WithText = !string.IsNullOrEmpty(text1);
            
        _addedModules.Add(component);
    }    
    public void AddPrice(string text)
    {
        var component = _modulesPool.SpawnModule(PopUpComponentType.Price) as PricePopUpComponentData;
        component.PriceTextContent = text;
        _addedModules.Add(component);
    }
    public void AddButton(string text, Action action, bool cond)
    {
        var component = _modulesPool.SpawnModule(PopUpComponentType.Button) as ButtonPopUpComponentData;
        component.ButtonText = text;
        component.OnButtonAction = action;
        component.CloseOnAction = cond;
        _addedModules.Add(component);
    }
    public void AddCloseButton()
    {
        var component = _modulesPool.SpawnModule(PopUpComponentType.CloseButton) as CloseButtonPopUpComponentData;
        _addedModules.Add(component);
    }

    public void Clear() { }
}
