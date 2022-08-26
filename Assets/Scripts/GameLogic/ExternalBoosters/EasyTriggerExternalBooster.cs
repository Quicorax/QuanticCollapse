using UnityEngine;

public class EasyTriggerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    const string EasyTrigger = "EasyTrigger";
    private ParticleSystem particlesEffect;
    //private StartshipScreenVisualEffects screenVisualEvents;

    private int lifeSubstractionAmount = 2;

    public EasyTriggerExternalBooster(MasterSceneManager master, ExternalBoosterElements elements, VirtualGridView view)
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
        //screenVisualEvents = elements.screenEffects;
        lifeSubstractionAmount = elements.valueDelta;
    }

    public override void Execute()
    {
        View.Controller.ModifyEnemyLife(-lifeSubstractionAmount);
        particlesEffect.Play();

        MasterSceneManager.Inventory.RemoveElement(EasyTrigger, 1);
        SetCountText();
        SetButtonInteractable();
    }
    void SetButtonInteractable() => ButtonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.Inventory.CheckElementAmount(EasyTrigger));
    public void SetCountText() => SetBoosterCountText(MasterSceneManager.Inventory.CheckElementAmount(EasyTrigger), TextRef);
}