using System;

public class ButtonPopUpComponentData : PopUpComponentData
{
    public Action OnButtonAction;
    public string ButtonText;

    public ButtonPopUpComponentData(string text, Action onButtonAction)
    {
        ModuleConcept = "Button";
        ModuleHeight = 150;

        ButtonText = text;
        OnButtonAction = onButtonAction;
    }
}
