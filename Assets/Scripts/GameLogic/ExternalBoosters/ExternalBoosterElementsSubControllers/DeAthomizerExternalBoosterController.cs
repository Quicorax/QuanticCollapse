
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DeAthomizer", menuName = "ScriptableObjects/ExternalBoosters/DeAthomizer")]
public class DeAthomizerExternalBoosterController : ExternalBooster, IExternalBooster
{
    private UserInputManager _inputManager;
    public ResourcesType BoosterType => ResourcesType.DeAthomizer;

    public void Execute(GridController Controller, Action<ResourcesType, bool> ConfirmExecution)
    {
        if(_inputManager == null)
            _inputManager = FindObjectOfType<UserInputManager>();

        _inputManager.DeAthomizerBoostedInput = !_inputManager.DeAthomizerBoostedInput;
        ConfirmExecution?.Invoke(BoosterType, _inputManager.DeAthomizerBoostedInput);
    }
}
