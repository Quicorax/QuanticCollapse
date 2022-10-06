using System;
using UnityEngine;

public class EasyTriggerExternalBoosterController : IExternalBooster
{
    public string BoosterId => "EasyTrigger";
    private int _lifeSubstractionAmount = 2;

    private ParticleSystem _particle;

    public void Execute(GridController Controller, Action<string, bool> ConfirmExecution)
    {
        Controller.ModifyEnemyLife(-_lifeSubstractionAmount);
        ConfirmExecution?.Invoke(BoosterId, true);

        if (_particle == null)
            _particle = GameObject.FindGameObjectWithTag("AttParticle").GetComponent<ParticleSystem>();

        _particle.Play();
    }
}