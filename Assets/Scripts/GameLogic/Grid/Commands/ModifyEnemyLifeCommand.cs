public class ModifyEnemyLifeCommand 
{
    private int _amount;
    private GenericEventBus _winConfitionEventBus;
    private GenericIntEventBus _enemyDamagedEventBus;
    public ModifyEnemyLifeCommand(GenericEventBus winEventBus, GenericIntEventBus enemyDamagedEventBus, int damage)
    {
        _winConfitionEventBus = winEventBus;
        _enemyDamagedEventBus = enemyDamagedEventBus;
        _amount = damage;
    }
    public void Do(GridModel Model)
    {
        if (!Model.IsEnemyMaxHealthSet)
        {
            Model.EnemyMaxHealth = _amount;
            Model.EnemyHealth = Model.EnemyMaxHealth;

            Model.IsEnemyMaxHealthSet = true;
        }
        else
            Model.EnemyHealth += _amount;

        _enemyDamagedEventBus.NotifyEvent(_amount);

        if (Model.EnemyHealth <= 0)
            _winConfitionEventBus.NotifyEvent();
    }
}
