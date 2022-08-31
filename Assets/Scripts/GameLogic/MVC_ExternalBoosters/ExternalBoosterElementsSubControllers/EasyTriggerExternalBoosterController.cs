using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EasyTrigger", menuName = "ScriptableObjects/ExternalBoosters/EasyTrigger")]
public class EasyTriggerExternalBoosterController : ExternalBoosterSourceController
{
    public int lifeSubstractionAmount = 2;
    public EasyTriggerExternalBoosterController()
    {
        boosterName = "EasyTrigger";
    }
    public override void Execute(VirtualGridController Controller, Action<string, bool> ConfirmExecution)
    {
        Controller.ModifyEnemyLife(-lifeSubstractionAmount);
        ConfirmExecution?.Invoke(boosterName, true);
    }
}