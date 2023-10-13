namespace QuanticCollapse
{
    public static class ModifyEnemyHealth
    {
        private static GenericEventBus _winConditionEventBus;
        private static GenericIntEventBus _enemyDamagedEventBus;
        private static GridModel _model;

        public static void Initialize(GridModel model, GenericEventBus winEventBus,
            GenericIntEventBus enemyDamagedEventBus)
        {
            _winConditionEventBus = winEventBus;
            _enemyDamagedEventBus = enemyDamagedEventBus;
            _model = model;
        }

        public static void Do(int damage)
        {
            if (!_model.IsEnemyMaxHealthSet)
            {
                _model.EnemyMaxHealth = damage;
                _model.EnemyHealth = _model.EnemyMaxHealth;

                _model.IsEnemyMaxHealthSet = true;
            }
            else
            {
                _model.EnemyHealth += damage;
            }

            _enemyDamagedEventBus.NotifyEvent(damage);

            if (_model.EnemyHealth <= 0)
            {
                _winConditionEventBus.NotifyEvent();
            }
        }
    }
}