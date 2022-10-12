using System;
using UnityEngine;

public class EasyTriggerExternalBoosterController : IExternalBooster
{
    public string BoosterId => "EasyTrigger";
    private int _lifeSubstractionAmount = 2;

    private ParticleSystem _particle;

    public void Execute(GridModel Model, Action<string, bool> ConfirmExecution)
    {
        Model.EnemyHealth -= _lifeSubstractionAmount; //TODO: This makes the ui not update

        ConfirmExecution?.Invoke(BoosterId, true);

        if (_particle == null)
            _particle = GameObject.FindGameObjectWithTag("AttParticle").GetComponent<ParticleSystem>();

        _particle.Play();
    }
}