using UnityEngine;

public class DeAthomizerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    private ParticleSystem particlesEffect;
    private StartshipScreenVisualEffects screenVisualEvents;
    public DeAthomizerExternalBooster(VirtualGridView view, MasterSceneManager master, ExternalBoosterElements elements)
    {
        base.View = view;
        base.MasterSceneManager = master;

        base.buttonRef = elements.buttonReference;
        base.textRef = elements.textRefeference;

        AddSpecificElements(elements);
        SetCountText();
    }

    void AddSpecificElements(ExternalBoosterElements elements)
    {
        particlesEffect = elements.particleEffectReference;
        screenVisualEvents = elements.screenEffects;
    }

    public void Execute()
    {
        MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount--;
        buttonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount);
        SetCountText();
    }
    public void SetCountText()
    {
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount, textRef);
    }
}
