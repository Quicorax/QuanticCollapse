public class PricePopUpComponentData : IPopUpComponentData
{
    public string PriceTextContent;

    public PricePopUpComponentData(string priceTextContent)
    { 
        PriceTextContent = priceTextContent;
    }

    public ModuleTypes ModuleConcept => ModuleTypes.Price;

    public int ModuleHeight => 150;
}