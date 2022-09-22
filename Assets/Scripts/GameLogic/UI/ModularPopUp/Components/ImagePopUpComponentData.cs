public class ImagePopUpComponentData : IPopUpComponentData
{
    public bool WithText;
    public string SpriteName;
    public string ImageText;

    public ImagePopUpComponentData(string spriteName, string imageText = null)
    {
        if(imageText != null)
        {
            WithText =true;
            ImageText = imageText;
        }
        SpriteName = spriteName;
    }

    public ModuleTypes ModuleConcept => ModuleTypes.Image;

    public int ModuleHeight => 200;
}
