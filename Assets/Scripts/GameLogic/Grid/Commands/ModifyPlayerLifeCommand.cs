public class ModifyPlayerLifeCommand : IGridCommand
{
    private int _amount;
    private GenericEventBus _loseConditionEventBus;
    private GenericIntEventBus _playerDamagedEventBus;
    public ModifyPlayerLifeCommand(GenericEventBus loseEventBus, GenericIntEventBus playerDamagedEventBus, int damage)
    {
        _loseConditionEventBus = loseEventBus;
        _playerDamagedEventBus = playerDamagedEventBus;
        _amount = damage;
    }
    public void Do(GridModel Model)
    {
        if (!Model.IsPlayerMaxHealthSet)
        {
            Model.PlayerMaxHealth = _amount;
            Model.PlayerHealth = Model.PlayerMaxHealth;

            Model.IsPlayerMaxHealthSet = true;
        }
        else
            Model.PlayerHealth += _amount;


        _playerDamagedEventBus.NotifyEvent(_amount);


        if (Model.PlayerHealth <= 0)
            _loseConditionEventBus.NotifyEvent();
    }
}
