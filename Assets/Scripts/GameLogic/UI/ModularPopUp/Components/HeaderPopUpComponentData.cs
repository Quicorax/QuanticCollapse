public class HeaderPopUpComponentData : PopUpComponentData
{
    public bool IsHeaderHighlighted;
    public string HeaderTextContent;

    public HeaderPopUpComponentData(string text = Constants.Empty, bool highlighted = false)
    {
        ModuleConcept = Constants.Header;
        ModuleHeight = 170;

        IsHeaderHighlighted = highlighted;
        HeaderTextContent = text;
    }
}
