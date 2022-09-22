public class HeaderPopUpComponentData : IPopUpComponentData
{
    public bool IsHeaderHighlighted;
    public string HeaderTextContent;

    public HeaderPopUpComponentData(string text = Constants.Empty, bool highlighted = false)
    {
        IsHeaderHighlighted = highlighted;
        HeaderTextContent = text;
    }

    public ModuleTypes ModuleConcept => ModuleTypes.Header;
    public int ModuleHeight => 170;
}
