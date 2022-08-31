
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
    private void Awake()
    {
        _inputManager = FindObjectOfType<UserInputManager>(); //TODO: Remove this Find
    }
    public override void Execute(VirtualGridController Controller, Action<string, bool> ConfirmExecution)
    {
        _inputManager.deAthomizerBoostedInput = !_inputManager.deAthomizerBoostedInput;
        ConfirmExecution?.Invoke(boosterName, _inputManager.deAthomizerBoostedInput);
    }
}
