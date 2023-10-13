using System;
using UnityEngine;

namespace QuanticCollapse
{
    public class DeAthomizerExternalBoosterController : IExternalBooster
    {
        public string BoosterId => "DeAthomizer";
        private UserInputManager _inputManager;

        public void Execute(GridModel gridModel, Action<string, bool> confirmExecution)
        {
            if (_inputManager == null)
            {
                _inputManager = GameObject.FindObjectOfType<UserInputManager>();
            }

            _inputManager.DeAthomizerBoostedInput = !_inputManager.DeAthomizerBoostedInput;
            confirmExecution?.Invoke(BoosterId, _inputManager.DeAthomizerBoostedInput);
        }
    }
}