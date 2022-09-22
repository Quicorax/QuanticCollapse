using System;
using UnityEngine;

public class CloseButtonPopUpComponentObject : MonoBehaviour, IPopUpComponentObject
{
    public ModuleTypes ModuleConcept;

    private Action OnButtonAction;

    public void SetData(IPopUpComponentData unTypedData, Action closeOnUse)
    {
        OnButtonAction = closeOnUse;
    }

    public void OnButtonPressed() => OnButtonAction?.Invoke();
}
