public class TextPopUpComponentData : IPopUpComponentData
{
    public string TextContent;

    public TextPopUpComponentData(string textContent = Constants.Empty)
    {
        TextContent = textContent;
    }

    public ModuleTypes ModuleConcept => ModuleTypes.Text;

    public int ModuleHeight => 170;
}
