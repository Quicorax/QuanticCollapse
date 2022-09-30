using System;
using System.Collections.Generic;
using UnityEngine;
public class PopUpService : IService
{
    private AddressablesService _addressables;
    public List<IPopUpComponentData> _addedModules = new();
    public ModulesPool pool = new();

    public void Initialize(AddressablesService addressables)
    {
        _addressables = addressables;
        pool.Initialize();

        PreloadModules();
    }

    private void PreloadModules()
    {
        AddHeader("", false);
        AddText("");
        AddImage("AllianceCredits", string.Empty);
        AddPrice("");
        AddButton("", null, false);
        AddCloseButton();

        SpawnPopUp(null);
    }
    public void SpawnPopUp(Transform parent) 
        => _addressables.SpawnAddressable<ModularPopUp>("Modular_PopUp", parent, x => x.GeneratePopUp(_addedModules));

    public void DeSpawnPopUp()
    {
        foreach (var item in _addedModules)
            pool.ReleaseModule(item.ModuleConcept, item);

        _addedModules.Clear();
    }
    public void AddHeader(string text, bool cond)
    {
        var component = pool.GetModule(PopUpComponentType.Header) as HeaderPopUpComponentData;
        component.HeaderTextContent = text;
        component.IsHeaderHighlighted = cond;
        _addedModules.Add(component);
    }
    public void AddText(string text)
    {
        var component = pool.GetModule(PopUpComponentType.Text) as TextPopUpComponentData;
        component.TextContent = text;
        _addedModules.Add(component);
    }
    public void AddImage(string text, string text1)
    {
        var component = pool.GetModule(PopUpComponentType.Image) as ImagePopUpComponentData;
        component.SpriteName = text;
        component.ImageText = text1;
        component.WithText = !string.IsNullOrEmpty(text1);
            
        _addedModules.Add(component);
    }    
    public void AddPrice(string text)
    {
        var component = pool.GetModule(PopUpComponentType.Price) as PricePopUpComponentData;
        component.PriceTextContent = text;
        _addedModules.Add(component);
    }
    public void AddButton(string text, Action action, bool cond)
    {
        var component = pool.GetModule(PopUpComponentType.Button) as ButtonPopUpComponentData;
        component.ButtonText = text;
        component.OnButtonAction = action;
        component.CloseOnAction = cond;
        _addedModules.Add(component);
    }
    public void AddCloseButton()
    {
        var component = pool.GetModule(PopUpComponentType.CloseButton) as CloseButtonPopUpComponentData;
        _addedModules.Add(component);
    }

    public void Clear() { }
}
