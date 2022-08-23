using UnityEngine;

public class DeAthomizerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    //private ParticleSystem particlesEffect;
    //private StartshipScreenVisualEffects screenVisualEvents;
    public DeAthomizerExternalBooster(MasterSceneManager master, ExternalBoosterElements elements,VirtualGridController controller)
    {
        Controller = controller;
        MasterSceneManager = master;
        ButtonRef = elements.buttonReference;
        TextRef = elements.textRefeference;

        //AddSpecificElements(elements);
        SetCountText();
        SetButtonInteractable();
    }

    //void AddSpecificElements(ExternalBoosterElements elements)
    //{
    //    particlesEffect = elements.particleEffectReference;
    //    screenVisualEvents = elements.screenEffects;
    //}

    public void Execute()
    {
        MasterSceneManager.SaveFiles.progres.deAthomizerBoosterAmount--;
        SetCountText();
        SetButtonInteractable();
    }
    void SetButtonInteractable() { ButtonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.SaveFiles.progres.deAthomizerBoosterAmount); }
    public void SetCountText() { SetBoosterCountText(MasterSceneManager.SaveFiles.progres.deAthomizerBoosterAmount, TextRef); }
}
