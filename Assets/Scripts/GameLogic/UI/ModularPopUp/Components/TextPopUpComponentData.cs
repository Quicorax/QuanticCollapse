public class TextPopUpComponentData : PopUpComponentData
{
    public string TextContent;

    public TextPopUpComponentData(string textContent)
    {
        ModuleConcept = "Text";
        ModuleHeight = 170;

        TextContent = textContent;
    }
}
