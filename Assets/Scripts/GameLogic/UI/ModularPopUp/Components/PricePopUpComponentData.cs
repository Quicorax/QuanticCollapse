public class PricePopUpComponentData : PopUpComponentData
{
    public string PriceTextContent;

    public PricePopUpComponentData(string priceTextContent = Constants.Empty)
    {
        ModuleConcept = Constants.Price;
        ModuleHeight = 150;

        PriceTextContent = priceTextContent;
    }
}