using UnityEngine;

public class FistAidExternalBooster : ExternalBoosterBase , IExternalBooster
{
    private ParticleSystem particlesEffect;
    private StartshipScreenVisualEffects screenVisualEvents;

    private int lifeRegenAmount = 5;

    public FistAidExternalBooster(MasterSceneManager master, ExternalBoosterElements elements, VirtualGridController controller)
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
        lifeRegenAmount = elements.valueDelta;
    }

    public void Execute()
    {
        if (Controller.CommandProcessor.Model.PlayerLife >= Controller.CommandProcessor.Model.playerMaxLife)
            return;

        Controller.ModifyPlayerLife(lifeRegenAmount);
        particlesEffect.Play();
        screenVisualEvents.Healed();

        MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount--;
        SetCountText();
        SetButtonInteractable();
    }

    void SetButtonInteractable()
    {
        ButtonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount);
    }
    public void SetCountText()
    {
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount, TextRef);
    }
}
