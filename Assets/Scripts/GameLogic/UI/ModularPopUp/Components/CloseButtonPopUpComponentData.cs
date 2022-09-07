using System;

public class CloseButtonPopUpComponentData : PopUpComponentData
{
    public Action OnButtonAction;
    public CloseButtonPopUpComponentData(Action onButtonAction)
    {
        ModuleConcept = "CloseButton";
        ModuleHeight = 0;

        OnButtonAction = onButtonAction;
    }

}
