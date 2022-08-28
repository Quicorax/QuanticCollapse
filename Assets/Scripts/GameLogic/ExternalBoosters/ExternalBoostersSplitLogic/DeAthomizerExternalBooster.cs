
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DeAthomizer", menuName = "ScriptableObjects/ExternalBoosters/DeAthomizer")]
public class DeAthomizerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    private UserInputManager _inputManager;
    public DeAthomizerExternalBooster()
    {
        boosterName = "DeAthomizer";
    }
    public override void Execute(VirtualGridView View, Action<string, bool> confirmExecution)
    {
        
        _inputManager = FindObjectOfType<UserInputManager>(); //TODO: Remove this Find

        _inputManager.deAthomizerBoostedInput = !_inputManager.deAthomizerBoostedInput;

        confirmExecution?.Invoke(boosterName, _inputManager.deAthomizerBoostedInput);
    }
}
