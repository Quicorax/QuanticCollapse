
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DeAthomizer", menuName = "ScriptableObjects/ExternalBoosters/DeAthomizer")]
public class DeAthomizerExternalBoosterController : ExternalBooster, IExternalBooster
{
    private UserInputManager _inputManager;
    public string BoosterId => "DeAthomizer";

    public void Execute(GridController Controller, Action<string, bool> ConfirmExecution)
    {
        if(_inputManager == null)
            _inputManager = FindObjectOfType<UserInputManager>();

        _inputManager.DeAthomizerBoostedInput = !_inputManager.DeAthomizerBoostedInput;
        ConfirmExecution?.Invoke(BoosterId, _inputManager.DeAthomizerBoostedInput);
    }
}
