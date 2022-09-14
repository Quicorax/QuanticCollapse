using System;

public class ButtonPopUpComponentData : PopUpComponentData
{
    public Action OnButtonAction;
    public string ButtonText;
    public bool CloseOnAction;
    public ButtonPopUpComponentData(string text, Action onButtonAction, bool closeOnAction)
    {
        ModuleConcept = "Button";
        ModuleHeight = 150;

        ButtonText = text;
        OnButtonAction = onButtonAction;
        CloseOnAction = closeOnAction;
    }
}
