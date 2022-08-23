using UnityEngine;

public class DeAthomizerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    private ParticleSystem particlesEffect;
    private StartshipScreenVisualEffects screenVisualEvents;
    public DeAthomizerExternalBooster(VirtualGridView view, MasterSceneManager master, ExternalBoosterElements elements)
    {
        base.View = view;
        base._MasterSceneManager = master;

        base.buttonRef = elements.buttonReference;
        base.textRef = elements.textRefeference;

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
        _MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount--;
        SetCountText();
        SetButtonInteractable();
    }
    void SetButtonInteractable()
    {
        buttonRef.interactable = CheckBoosterNotEmpty(_MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount);
    }
    public void SetCountText()
    {
        SetBoosterCountText(_MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount, textRef);
    }
}
