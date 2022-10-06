using System;

public class FirstAidKitExternalBoosterController : IExternalBooster
{
    public string BoosterId => "FirstAidKit";
    private int _lifeRegenAmount = 5;

    public void Execute(GridController Controller, Action<string, bool> ConfirmExecution)
    {
        if (Controller.Model.PlayerHealth < Controller.Model.PlayerMaxHealth)
        {
            Controller.ModifyPlayerLife(_lifeRegenAmount);
            ConfirmExecution?.Invoke(BoosterId, true);
        }
    }
}
