public class HeaderPopUpComponentData : PopUpComponentData
{
    public bool IsHeaderHighlighted;
    public string HeaderTextContent;

    public HeaderPopUpComponentData(string text, bool highlighted)
    {
        ModuleConcept = "Header";
        ModuleHeight = 170;

        IsHeaderHighlighted = highlighted;
        HeaderTextContent = text;
    }
}
