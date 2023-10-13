using System;
using UnityEngine;

namespace QuanticCollapse
{
    public class EasyTriggerExternalBoosterController : IExternalBooster
    {
        public string BoosterId => "EasyTrigger";
        private readonly int _lifeSubstractionAmount = 2;

        private ParticleSystem _particle;

        public void Execute(GridModel gridModel, Action<string, bool> confirmExecution)
        {
            ModifyEnemyHealth.Do(-_lifeSubstractionAmount);

            confirmExecution?.Invoke(BoosterId, true);

            if (_particle == null)
            {
                _particle = GameObject.FindGameObjectWithTag("AttParticle").GetComponent<ParticleSystem>();
            }

            _particle.Play();
        }
    }
}