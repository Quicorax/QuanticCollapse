using System;

public class CloseButtonPopUpComponentObject : PopUpComponentObject
{
    private Action OnButtonAction;
    public override void SetData(PopUpComponentData unTypedData, Action closeOnUse)
    {
        OnButtonAction = closeOnUse;
    }

    public void OnButtonPressed() => OnButtonAction?.Invoke();
}
