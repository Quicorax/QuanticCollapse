using System;
using UnityEngine;

public class CloseButtonPopUpComponentObject : PopUpComponentObject
{
    private Action OnButtonAction;
    public override void SetData(PopUpComponentData unTypedData)
    {
        CloseButtonPopUpComponentData data = unTypedData as CloseButtonPopUpComponentData;

        OnButtonAction = data.OnButtonAction;
    }

    public void OnButtonPressed()
    {
        OnButtonAction?.Invoke();
    }
}
