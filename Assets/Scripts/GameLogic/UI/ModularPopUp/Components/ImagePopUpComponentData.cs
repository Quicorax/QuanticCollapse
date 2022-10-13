
namespace QuanticCollapse
{
    public class ImagePopUpComponentData : IPopUpComponentData
    {
        public bool WithText;
        public string SpriteName;
        public string ImageText;

        public PopUpComponentType ModuleConcept => PopUpComponentType.Image;

        public int ModuleHeight => 200;
    }
}