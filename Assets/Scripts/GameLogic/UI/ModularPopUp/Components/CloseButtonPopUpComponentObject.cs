using System;
using UnityEngine;

namespace QuanticCollapse
{
    public class CloseButtonPopUpComponentObject : MonoBehaviour, IPopUpComponentObject
    {
        public PopUpComponentType ModuleConcept;

        private Action _onButtonAction;

        public void SetData(IPopUpComponentData unTypedData, Action closeOnUse)
        {
            _onButtonAction = closeOnUse;
        }

        public void OnButtonPressed() => _onButtonAction?.Invoke();
    }
}