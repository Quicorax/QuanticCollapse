using System;
using UnityEngine;

namespace QuanticCollapse
{
    public class PopUpService : IService
    {
        private AddressablesService _addressables;
        private ModulesPool _modulesPool = new();
        private IPopUpComponentData[] _components;
        public void Initialize(AddressablesService addressables)
        {
            _addressables = addressables;
            _modulesPool.Initialize();

            PreloadModules();
        }

        public void SpawnPopUp(Transform parent, IPopUpComponentData[] components)
        {
            _components = components;
            _addressables.LoadAdrsOfComponent<ModularPopUp>("Modular_PopUp", parent, popUp => 
                popUp.GeneratePopUp(components));
        }

        public void DeSpawnPopUp()
        {
            foreach (var item in _components)
                _modulesPool.DeSpawnModule(item.ModuleConcept, item);

            _components = null;
        }

        #region Components
        public HeaderPopUpComponentData AddHeader(string text, bool cond)
        {
            var component = _modulesPool.SpawnModule(PopUpComponentType.Header) as HeaderPopUpComponentData;
            component.HeaderTextContent = text;
            component.IsHeaderHighlighted = cond;

            return component;
        }
        public TextPopUpComponentData AddText(string text)
        {
            var component = _modulesPool.SpawnModule(PopUpComponentType.Text) as TextPopUpComponentData;
            component.TextContent = text;

            return component;
        }
        public ImagePopUpComponentData AddImage(string text, string text1)
        {
            var component = _modulesPool.SpawnModule(PopUpComponentType.Image) as ImagePopUpComponentData;
            component.SpriteName = text;
            component.ImageText = text1;
            component.WithText = !string.IsNullOrEmpty(text1);

            return component;
        }
        public PricePopUpComponentData AddPrice(string text)
        {
            var component = _modulesPool.SpawnModule(PopUpComponentType.Price) as PricePopUpComponentData;
            component.PriceTextContent = text;

            return component;
        }
        public ButtonPopUpComponentData AddButton(string text, Action action, bool cond)
        {
            var component = _modulesPool.SpawnModule(PopUpComponentType.Button) as ButtonPopUpComponentData;
            component.ButtonText = text;
            component.OnButtonAction = action;
            component.CloseOnAction = cond;

            return component;
        }
        public CloseButtonPopUpComponentData AddCloseButton()
        {
            var component = _modulesPool.SpawnModule(PopUpComponentType.CloseButton) as CloseButtonPopUpComponentData;

            return component;
        }
        #endregion

        private void PreloadModules()
        {
            SpawnPopUp(null, new IPopUpComponentData[]
            {
                AddHeader(string.Empty, false),
                AddText(string.Empty),
                AddImage("AllianceCredits", string.Empty),
                AddPrice(string.Empty),
                AddButton(string.Empty, null, false),
                AddCloseButton()
            });
        }

        public void Clear() { }
    }
}