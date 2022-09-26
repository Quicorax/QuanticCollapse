public class TextPopUpComponentData : IPopUpComponentData
{
    public string TextContent;

    public TextPopUpComponentData(string textContent)
    {
        TextContent = textContent;
    }

    public ModuleTypes ModuleConcept => ModuleTypes.Text;

    public int ModuleHeight => 170;
}
