
using UnityEngine;

public class FistAidKidExternalBoosterLogic : ExternalBoosters
{
    public int lifeRegenAmount = 5;
    public ParticleSystem particlesEffect;
    public StartshipScreenVisualEvents screenVisualEvents;
    public void Execute()
    {
        if (View.Controller.Model.PlayerLife >= View.Controller.Model.playerMaxLife)
            return;

        Used();
        View.ModifyPlayerLife(lifeRegenAmount);
        particlesEffect.Play();
        screenVisualEvents.Healed();
    }
}
