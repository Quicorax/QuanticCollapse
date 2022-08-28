using UnityEngine.UI;

public class ModifyPlayerLifeCommand : IGridCommand
{
    private int _damageAmount;
    private GenericEventBus _loseConditionEventBus;
    private GenericIntEventBus _playerDamagedEventBus;
    public ModifyPlayerLifeCommand(GenericEventBus loseEventBus, GenericIntEventBus playerDamagedEventBus, int damage)
    {
        _loseConditionEventBus = loseEventBus;
        _playerDamagedEventBus = playerDamagedEventBus;
        _damageAmount = damage;
    }
    public void Do(VirtualGridModel Model)
    {
        Model.PlayerLife += _damageAmount;
        _playerDamagedEventBus.NotifyEvent(_damageAmount);


        if (Model.PlayerLife <= 0)
        {
            _loseConditionEventBus.NotifyEvent();
        }
    }
}
