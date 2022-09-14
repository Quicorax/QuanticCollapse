public class PricePopUpComponentData : PopUpComponentData
{
    public string PriceTextContent;

    public PricePopUpComponentData(string priceTextContent)
    {
        ModuleConcept = "Price";
        ModuleHeight = 150;

        PriceTextContent = priceTextContent;
    }
}