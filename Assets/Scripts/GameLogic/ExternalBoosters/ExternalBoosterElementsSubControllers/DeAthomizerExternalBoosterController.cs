
using System;
using UnityEngine;

namespace QuanticCollapse
{
    public class DeAthomizerExternalBoosterController : IExternalBooster
    {
        public string BoosterId => "DeAthomizer";
        private UserInputManager _inputManager;

        public void Execute(GridModel Model, Action<string, bool> ConfirmExecution)
        {
            if (_inputManager == null)
                _inputManager = GameObject.FindObjectOfType<UserInputManager>();

            _inputManager.DeAthomizerBoostedInput = !_inputManager.DeAthomizerBoostedInput;
            ConfirmExecution?.Invoke(BoosterId, _inputManager.DeAthomizerBoostedInput);
        }
    }
}