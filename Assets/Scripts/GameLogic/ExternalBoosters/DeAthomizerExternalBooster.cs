
public class DeAthomizerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    const string DeAthomizer = "DeAthomizer";
    //private ParticleSystem particlesEffect;
    //private StartshipScreenVisualEffects screenVisualEvents;
    public DeAthomizerExternalBooster(MasterSceneManager master, ExternalBoosterElements elements, VirtualGridView view)
    {
        View = view;
        MasterSceneManager = master;
        ButtonRef = elements.buttonReference;
        TextRef = elements.textRefeference;

        //AddSpecificElements(elements);
        SetCountText();
        SetButtonInteractable();
    }

    //void AddSpecificElements(ExternalBoosterElements elements)
    //{
    //    particlesEffect = elements.particleEffectReference;
    //    screenVisualEvents = elements.screenEffects;
    //}

    public void Execute()
    {
        MasterSceneManager.Inventory.RemoveElement(DeAthomizer, 1);
        SetCountText();
        SetButtonInteractable();
    }
    void SetButtonInteractable() { ButtonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.Inventory.CheckElementAmount(DeAthomizer)); }
    public void SetCountText() { SetBoosterCountText(MasterSceneManager.Inventory.CheckElementAmount(DeAthomizer), TextRef); }
}
