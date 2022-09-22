using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EasyTrigger", menuName = "ScriptableObjects/ExternalBoosters/EasyTrigger")]
public class EasyTriggerExternalBoosterController : ExternalBoosterSourceController
{
    public int lifeSubstractionAmount = 2;
    private ParticleSystem particle;

    public EasyTriggerExternalBoosterController()
    {
        boosterName = Constants.EasyTrigger;
    }

    public override void Execute(GridController Controller, Action<string, bool> ConfirmExecution)
    {
        Controller.ModifyEnemyLife(-lifeSubstractionAmount);
        ConfirmExecution?.Invoke(boosterName, true);

        if (particle == null)
            particle = GameObject.FindGameObjectWithTag(Constants.AttackParticle).GetComponent<ParticleSystem>(); //TODO: Remove this Find

        particle.Play();
    }
}