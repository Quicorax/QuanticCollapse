
namespace QuanticCollapse
{
    public static class ModifyPlayerHealth
    {
        private static GenericEventBus _loseConditionEventBus;
        private static GenericIntEventBus _playerDamagedEventBus;
        private static GridModel _model;

        public static void Initialize(GridModel model, GenericEventBus loseEventBus, GenericIntEventBus playerDamagedEventBus)
        {
            _loseConditionEventBus = loseEventBus;
            _playerDamagedEventBus = playerDamagedEventBus;
            _model = model;
        }
        public static void Do(int damage)
        {
            if (!_model.IsPlayerMaxHealthSet)
            {
                _model.PlayerMaxHealth = damage;
                _model.PlayerHealth = _model.PlayerMaxHealth;

                _model.IsPlayerMaxHealthSet = true;
            }
            else
            {
                if (_model.PlayerHealth + damage > _model.PlayerMaxHealth)
                    _model.PlayerHealth = _model.PlayerMaxHealth;
                else
                    _model.PlayerHealth += damage;
            }

            _playerDamagedEventBus.NotifyEvent(damage);

            if (_model.PlayerHealth <= 0)
                _loseConditionEventBus.NotifyEvent();
        }
    }
}