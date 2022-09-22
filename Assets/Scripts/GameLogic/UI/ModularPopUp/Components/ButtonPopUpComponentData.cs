using System;

public class ButtonPopUpComponentData : IPopUpComponentData
{
    public Action OnButtonAction;
    public string ButtonText;
    public bool CloseOnAction;
    public ButtonPopUpComponentData(string text = Constants.Empty, Action onButtonAction = null, bool closeOnAction = false)
    {
        ButtonText = text;
        OnButtonAction = onButtonAction;
        CloseOnAction = closeOnAction;
    }

    public ModuleTypes ModuleConcept => ModuleTypes.Button;

    public int ModuleHeight => 150;
}
