using System;

namespace QuanticCollapse
{
    public class ButtonPopUpComponentData : IPopUpComponentData
    {
        public string ButtonText;
        public Action OnButtonAction;
        public bool CloseOnAction;

        public PopUpComponentType ModuleConcept => PopUpComponentType.Button;

        public int ModuleHeight => 150;
    }
}