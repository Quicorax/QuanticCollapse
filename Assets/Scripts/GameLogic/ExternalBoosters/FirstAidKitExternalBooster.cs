using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FirstAidKit", menuName = "ScriptableObjects/ExternalBoosters/FirstAidKit")]
public class FirstAidKitExternalBooster : ExternalBoosterBase , IExternalBooster
{
    public int lifeRegenAmount = 5;
    public FirstAidKitExternalBooster()
    {
        boosterName = "FistAidKit";
    }

    public override void Execute(VirtualGridView View, Action<string, bool> confirmExecution)
    {
        if (View.Controller.Model.PlayerLife < View.Controller.Model.playerMaxLife)
        {
            View.Controller.ModifyPlayerLife(lifeRegenAmount);
            confirmExecution?.Invoke(boosterName, true);
        }
    }
}
