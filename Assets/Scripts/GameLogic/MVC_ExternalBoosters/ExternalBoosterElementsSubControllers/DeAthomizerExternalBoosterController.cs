
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DeAthomizer", menuName = "ScriptableObjects/ExternalBoosters/DeAthomizer")]
public class DeAthomizerExternalBoosterController : ExternalBoosterSourceController
{
    private UserInputManager _inputManager;
    public DeAthomizerExternalBoosterController()
    {
        boosterName = "DeAthomizer";
    }
    public override void Execute(GridController Controller, Action<string, bool> ConfirmExecution)
    {
        _inputManager = FindObjectOfType<UserInputManager>(); //TODO: Remove this Find

        _inputManager.deAthomizerBoostedInput = !_inputManager.deAthomizerBoostedInput;
        ConfirmExecution?.Invoke(boosterName, _inputManager.deAthomizerBoostedInput);
    }
}
