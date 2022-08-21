using TMPro;
using UnityEngine;

public class FistAidExternalBooster : ExternalBoosterBase , IExternalBooster
{
    private ParticleSystem particlesEffect;
    private StartshipScreenVisualEffects screenVisualEvents;

    private int lifeRegenAmount = 5;

    public FistAidExternalBooster(VirtualGridView view, MasterSceneManager master, ExternalBoosterElements elements)
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
        lifeRegenAmount = elements.valueDelta;
    }

    public void Execute()
    {
        View.ModifyPlayerLife(lifeRegenAmount);
        particlesEffect.Play();
        screenVisualEvents.Healed();

        _MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount--;
        SetCountText();
        SetButtonInteractable();
    }

    void SetButtonInteractable()
    {
        buttonRef.interactable = CheckBoosterNotEmpty(_MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount);
    }
    public void SetCountText()
    {
        SetBoosterCountText(_MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount, textRef);
    }
}
