using System;
using TMPro;
using UnityEngine;

public class ButtonPopUpComponentObject : PopUpComponentObject
{
    [SerializeField] private TMP_Text ButtonTextObject;

    private Action OnButtonAction;
    public override void SetData(PopUpComponentData unTypedData)
    {
        ButtonPopUpComponentData data = unTypedData as ButtonPopUpComponentData;

        ButtonTextObject.text = data.ButtonText;
        OnButtonAction = data.OnButtonAction;
    }

    public void OnButtonPressed() => OnButtonAction?.Invoke();
}
