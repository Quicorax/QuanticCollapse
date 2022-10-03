using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EasyTrigger", menuName = "ScriptableObjects/ExternalBoosters/EasyTrigger")]
public class EasyTriggerExternalBoosterController : ExternalBooster, IExternalBooster
{
    public int lifeSubstractionAmount = 2;
    private ParticleSystem particle;

    public string BoosterId => "EasyTrigger";

    public void Execute(GridController Controller, Action<string, bool> ConfirmExecution)
    {
        Controller.ModifyEnemyLife(-lifeSubstractionAmount);
        ConfirmExecution?.Invoke(BoosterId, true);

        if (particle == null)
            particle = GameObject.FindGameObjectWithTag("AttParticle").GetComponent<ParticleSystem>();

        particle.Play();
    }
}