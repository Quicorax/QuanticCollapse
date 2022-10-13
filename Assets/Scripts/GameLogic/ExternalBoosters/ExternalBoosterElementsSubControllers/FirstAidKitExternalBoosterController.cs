using System;

namespace QuanticCollapse
{
    public class FirstAidKitExternalBoosterController : IExternalBooster
    {
        public string BoosterId => "FirstAidKit";
        private int _lifeRegenAmount = 5;

        public void Execute(GridModel Model, Action<string, bool> ConfirmExecution)
        {
            if (Model.PlayerHealth < Model.PlayerMaxHealth)
            {
                ModifyPlayerHealth.Do(_lifeRegenAmount);
                ConfirmExecution?.Invoke(BoosterId, true);
            }
        }
    }
}