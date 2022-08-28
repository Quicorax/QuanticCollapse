using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EasyTrigger", menuName = "ScriptableObjects/ExternalBoosters/EasyTrigger")]
public class EasyTriggerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    public int lifeSubstractionAmount = 2;
    public EasyTriggerExternalBooster()
    {
        boosterName = "EasyTrigger";
    }
    public override void Execute(VirtualGridView View, Action<string, bool> confirmExecution)
    {
        View.Controller.ModifyEnemyLife(-lifeSubstractionAmount);
        confirmExecution?.Invoke(boosterName, true);
    }
}