using UnityEngine;

public class EasyTriggerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    private ParticleSystem particlesEffect;
    private StartshipScreenVisualEffects screenVisualEvents;

    private int lifeSubstractionAmount = 2;

    public EasyTriggerExternalBooster(VirtualGridView view, MasterSceneManager master, ExternalBoosterElements elements)
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
        lifeSubstractionAmount = elements.valueDelta;
    }

    public void Execute()
    {
        View.ModifyEnemyLife(-lifeSubstractionAmount);
        particlesEffect.Play();

        MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount--;
        SetCountText();
        buttonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount);
    }

    public void SetCountText()
    {
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount, textRef);
    }
}