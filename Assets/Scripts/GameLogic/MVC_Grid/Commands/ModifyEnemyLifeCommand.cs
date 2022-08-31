using UnityEngine.UI;

public class ModifyEnemyLifeCommand : IGridCommand
{
    private int _damageAmount;
    private GenericEventBus _winConfitionEventBus;
    private GenericIntEventBus _enemyDamagedEventBus;
    public ModifyEnemyLifeCommand(GenericEventBus winEventBus, GenericIntEventBus enemyDamagedEventBus, int damage)
    {
        _winConfitionEventBus = winEventBus;
        _enemyDamagedEventBus = enemyDamagedEventBus;
        _damageAmount = damage;
    }
    public void Do(GridModel Model)
    {
        Model.EnemyLife += _damageAmount;
        _enemyDamagedEventBus.NotifyEvent(_damageAmount);

        if (Model.EnemyLife <= 0)
        {
            _winConfitionEventBus.NotifyEvent();
        }
    }
}
