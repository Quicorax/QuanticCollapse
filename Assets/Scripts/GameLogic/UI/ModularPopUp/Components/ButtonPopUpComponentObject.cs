using System;
using TMPro;
using UnityEngine;

public class ButtonPopUpComponentObject : PopUpComponentObject
{
    [SerializeField] private TMP_Text ButtonTextObject;

    private Action OnButtonAction;
    private Action OnButtonClose;
    private bool _closeOnAction;
    public override void SetData(PopUpComponentData unTypedData, Action closeOnUse)
    {
        ButtonPopUpComponentData data = unTypedData as ButtonPopUpComponentData;

        ButtonTextObject.text = data.ButtonText;
        OnButtonAction = data.OnButtonAction;
        _closeOnAction = data.CloseOnAction;
        OnButtonClose = closeOnUse;
    }

    public void OnButtonPressed()
    {
        OnButtonAction?.Invoke();

        if(_closeOnAction)
            OnButtonClose?.Invoke();
    }
}
