public class TextPopUpComponentData : PopUpComponentData
{
    public string TextContent;

    public TextPopUpComponentData(string textContent = Constants.Empty)
    {
        ModuleConcept = Constants.Text;
        ModuleHeight = 170;

        TextContent = textContent;
    }
}
