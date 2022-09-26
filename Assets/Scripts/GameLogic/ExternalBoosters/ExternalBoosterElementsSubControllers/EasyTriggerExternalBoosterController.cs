using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EasyTrigger", menuName = "ScriptableObjects/ExternalBoosters/EasyTrigger")]
public class EasyTriggerExternalBoosterController : ExternalBooster, IExternalBooster
{
    public int lifeSubstractionAmount = 2;
    private ParticleSystem particle;

    public ResourcesType BoosterType => ResourcesType.EasyTrigger;

    public void Execute(GridController Controller, Action<ResourcesType, bool> ConfirmExecution)
    {
        Controller.ModifyEnemyLife(-lifeSubstractionAmount);
        ConfirmExecution?.Invoke(BoosterType, true);

        if (particle == null)
            particle = GameObject.FindGameObjectWithTag("AttParticle").GetComponent<ParticleSystem>(); //TODO: Remove this Find

        particle.Play();
    }
}