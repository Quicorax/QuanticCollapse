using UnityEngine;

public class DeAthomizerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    private ParticleSystem particlesEffect;
    private StartshipScreenVisualEffects screenVisualEvents;
    public DeAthomizerExternalBooster(MasterSceneManager master, ExternalBoosterElements elements,VirtualGridController controller)
    {
        base.Controller = controller;
        base.MasterSceneManager = master;
        base.ButtonRef = elements.buttonReference;
        base.TextRef = elements.textRefeference;

        AddSpecificElements(elements);
        SetCountText();
        SetButtonInteractable();
    }

    void AddSpecificElements(ExternalBoosterElements elements)
    {
        particlesEffect = elements.particleEffectReference;
        screenVisualEvents = elements.screenEffects;
    }

    public void Execute()
    {
        MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount--;
        SetCountText();
        SetButtonInteractable();
    }
    void SetButtonInteractable()
    {
        ButtonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount);
    }
    public void SetCountText()
    {
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount, TextRef);
    }
}
