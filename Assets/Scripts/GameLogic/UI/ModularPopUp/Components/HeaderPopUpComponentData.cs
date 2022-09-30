public class HeaderPopUpComponentData : IPopUpComponentData
{
    public bool IsHeaderHighlighted;
    public string HeaderTextContent;

    public PopUpComponentType ModuleConcept => PopUpComponentType.Header;
    public int ModuleHeight => 170;
}
