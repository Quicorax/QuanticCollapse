using System;

public class ButtonPopUpComponentData : PopUpComponentData
{
    public Action OnButtonAction;
    public string ButtonText;
    public bool CloseOnAction;
    public ButtonPopUpComponentData(string text = Constants.Empty, Action onButtonAction = null, bool closeOnAction = false)
    {
        ModuleConcept = Constants.Button;
        ModuleHeight = 150;

        ButtonText = text;
        OnButtonAction = onButtonAction;
        CloseOnAction = closeOnAction;
    }
}
