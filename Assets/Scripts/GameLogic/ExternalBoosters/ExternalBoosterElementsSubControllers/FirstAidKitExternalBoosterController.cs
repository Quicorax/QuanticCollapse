using System;

namespace QuanticCollapse
{
    public class FirstAidKitExternalBoosterController : IExternalBooster
    {
        public string BoosterId => "FirstAidKit";
        private readonly int _lifeRegenAmount = 5;

        public void Execute(GridModel gridModel, Action<string, bool> confirmExecution)
        {
            if (gridModel.PlayerHealth < gridModel.PlayerMaxHealth)
            {
                ModifyPlayerHealth.Do(_lifeRegenAmount);
                confirmExecution?.Invoke(BoosterId, true);
            }
        }
    }
}