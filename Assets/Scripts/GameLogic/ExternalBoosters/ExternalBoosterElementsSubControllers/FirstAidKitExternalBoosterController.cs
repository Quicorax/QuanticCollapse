using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FirstAidKit", menuName = "ScriptableObjects/ExternalBoosters/FirstAidKit")]
public class FirstAidKitExternalBoosterController : ExternalBooster, IExternalBooster
{
    public int lifeRegenAmount = 5;

    public string BoosterId => "FirstAidKit";

    public void Execute(GridController Controller, Action<string, bool> ConfirmExecution)
    {
        if (Controller.Model.PlayerHealth < Controller.Model.PlayerMaxHealth)
        {
            Controller.ModifyPlayerLife(lifeRegenAmount);
            ConfirmExecution?.Invoke(BoosterId, true);
        }
    }
}
