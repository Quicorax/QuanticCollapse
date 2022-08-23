using UnityEngine;

public class EasyTriggerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    private ParticleSystem particlesEffect;
    //private StartshipScreenVisualEffects screenVisualEvents;

    private int lifeSubstractionAmount = 2;

    public EasyTriggerExternalBooster(MasterSceneManager master, ExternalBoosterElements elements, VirtualGridController controller)
    {
        Controller = controller;
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

    public void Execute()
    {
        Controller.ModifyEnemyLife(-lifeSubstractionAmount);
        particlesEffect.Play();

        MasterSceneManager.SaveFiles.progres.easyTriggerBoosterAmount--;
        SetCountText();
        SetButtonInteractable();
    }
    void SetButtonInteractable() { ButtonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.SaveFiles.progres.easyTriggerBoosterAmount); }
    public void SetCountText() { SetBoosterCountText(MasterSceneManager.SaveFiles.progres.easyTriggerBoosterAmount, TextRef); }
}