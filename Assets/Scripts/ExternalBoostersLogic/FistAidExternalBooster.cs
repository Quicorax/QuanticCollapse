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
        lifeRegenAmount = elements.valueDelta;
    }

    public void Execute()
    {
        View.ModifyPlayerLife(lifeRegenAmount);
        particlesEffect.Play();
        screenVisualEvents.Healed();

        MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount--;
        SetCountText();
        buttonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount);
    }
    public void SetCountText()
    {
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount, textRef);
    }
}
