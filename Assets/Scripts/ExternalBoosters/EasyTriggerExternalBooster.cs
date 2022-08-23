using UnityEngine;

public class EasyTriggerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    private ParticleSystem particlesEffect;
    private StartshipScreenVisualEffects screenVisualEvents;

    private int lifeSubstractionAmount = 2;

    public EasyTriggerExternalBooster(VirtualGridView view, MasterSceneManager master, ExternalBoosterElements elements)
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
        lifeSubstractionAmount = elements.valueDelta;
    }

    public void Execute()
    {
        View.ModifyEnemyLife(-lifeSubstractionAmount);
        particlesEffect.Play();

        _MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount--;
        SetCountText();
        SetButtonInteractable();
    }
    void SetButtonInteractable()
    {
        buttonRef.interactable = CheckBoosterNotEmpty(_MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount);
    }
    public void SetCountText()
    {
        SetBoosterCountText(_MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount, textRef);
    }
}