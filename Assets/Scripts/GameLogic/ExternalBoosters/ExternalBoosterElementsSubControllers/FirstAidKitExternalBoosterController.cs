using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FirstAidKit", menuName = "ScriptableObjects/ExternalBoosters/FirstAidKit")]
public class FirstAidKitExternalBoosterController : ExternalBoosterSourceController
{
    public int lifeRegenAmount = 5;
    public FirstAidKitExternalBoosterController()
    {
        boosterType = ResourcesType.FirstAidKit;
    }

    public override void Execute(GridController Controller, Action<ResourcesType, bool> ConfirmExecution)
    {
        if (Controller.Model.PlayerHealth < Controller.Model.PlayerMaxHealth)
        {
            Controller.ModifyPlayerLife(lifeRegenAmount);
            ConfirmExecution?.Invoke(boosterType, true);
        }
    }
}
