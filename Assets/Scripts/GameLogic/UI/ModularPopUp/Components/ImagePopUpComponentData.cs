public class ImagePopUpComponentData : PopUpComponentData
{
    public bool WithText;
    public string SpriteName;
    public string ImageText;

    public ImagePopUpComponentData(string spriteName, string imageText = null)
    {
        ModuleConcept = "Image";
        ModuleHeight = 200;

        if(imageText != null)
        {
            WithText =true;
            ImageText = imageText;
        }
        SpriteName = spriteName;
    }
}
