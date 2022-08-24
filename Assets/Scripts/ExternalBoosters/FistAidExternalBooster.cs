using UnityEngine;

public class FistAidExternalBooster : ExternalBoosterBase , IExternalBooster
{
    const string FistAidKit = "FistAidKit";

    private ParticleSystem particlesEffect;
    private StartshipScreenVisualEffects screenVisualEvents;

    private int lifeRegenAmount = 5;

    public FistAidExternalBooster(MasterSceneManager master, ExternalBoosterElements elements, VirtualGridView view)
    {
        View = view;
        MasterSceneManager = master;
        ButtonRef = elements.buttonReference;
        TextRef = elements.textRefeference;

        AddSpecificElements(elements);
        SetCountText();
        SetButtonInteractable();
    }

    void AddSpecificElements(ExternalBoosterElements elements)
    {
        particlesEffect = elements.particleEffectReference;
        screenVisualEvents = elements.screenEffects;
        lifeRegenAmount = elements.valueDelta;
    }

    public void Execute()
    {
        if (View.Controller.CommandProcessor.Model.PlayerLife >= View.Controller.CommandProcessor.Model.playerMaxLife)
            return;

        View.Controller.ModifyPlayerLife(lifeRegenAmount);
        particlesEffect.Play();
        screenVisualEvents.Healed();

        MasterSceneManager.Inventory.RemoveElement(FistAidKit, 1);
        SetCountText();
        SetButtonInteractable();
    }

    void SetButtonInteractable() { ButtonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.Inventory.CheckElementAmount(FistAidKit)); }
    public void SetCountText() { SetBoosterCountText(MasterSceneManager.Inventory.CheckElementAmount(FistAidKit), TextRef); }
}
