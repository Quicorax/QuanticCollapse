namespace QuanticCollapse
{
    public class PricePopUpComponentData : IPopUpComponentData
    {
        public string PriceTextContent;

        public PopUpComponentType ModuleConcept => PopUpComponentType.Price;

        public int ModuleHeight => 150;
    }
}