using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace QuanticCollapse
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private ExternalBoosterScreenEffectEventBus _ScreenEffects;
        [SerializeField] private LevelInjectedEventBus _LevelInjected;
        [SerializeField] private GenericEventBus _PoolLoaded;
        [SerializeField] private GenericEventBus _WinConfitionEventBus;
        [SerializeField] private GenericIntEventBus _EnemyDamagedEventBus;
        [SerializeField] private GenericIntEventBus _PlayerDamagedEventBus;
        [SerializeField] private AddScoreEventBus _AddScoreEventBus;
        [SerializeField] private GenericEventBus _BlockDestructionEventBus;

        [SerializeField] private PoolManager _poolManager;
        [SerializeField] private UserInputManager _userInputManager;
        [SerializeField] private TurnManager _turnManager;

        [SerializeField] private PlayerStarshipData _playerData;
        [SerializeField] private EnemyStarshipData _enemyData;
        [SerializeField] private Transform externalBoosterParent;

        [SerializeField] private Slider _enemyLifeSlider;
        [SerializeField] private Slider _playerLifeSlider;

        private GridController _gridController;
        private GridModel _gridModel;

        private LevelModel _levelData;

        private AnalyticsGameService _analytics;

        private void Awake()
        {
            _EnemyDamagedEventBus.Event += EnemyDamaged;
            _PlayerDamagedEventBus.Event += PlayerDamaged;
            _LevelInjected.Event += SetLevelData;
            _PoolLoaded.Event += Generate;

            _analytics = ServiceLocator.GetService<AnalyticsGameService>();

            _gridModel = new();
            _gridController = new(_gridModel, _AddScoreEventBus, _BlockDestructionEventBus, _poolManager,
                _userInputManager, _turnManager);
        }

        private void OnDestroy()
        {
            _EnemyDamagedEventBus.Event -= EnemyDamaged;
            _PlayerDamagedEventBus.Event -= PlayerDamaged;
            _LevelInjected.Event -= SetLevelData;
            _PoolLoaded.Event -= Generate;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            ExternalBoosterView externalBoosterView = new();
            externalBoosterView.Initialize(_ScreenEffects, _gridModel, externalBoosterParent);

            ModifyEnemyHealth.Initialize(_gridModel, _WinConfitionEventBus, _EnemyDamagedEventBus);
            ModifyEnemyHealth.Do(_enemyData.StarshipHealth);

            ModifyPlayerHealth.Initialize(_gridModel, _WinConfitionEventBus, _PlayerDamagedEventBus);
            ModifyPlayerHealth.Do(_playerData.StarshipHealth);
        }

        public void ProcessInput(Vector2Int inputCoords, bool boostedInput) =>
            _gridController.Interact(inputCoords, boostedInput).ManageTaskException();

        private void PlayerDamaged(int amount) => _playerLifeSlider.value += amount;
        private void EnemyDamaged(int amount) => _enemyLifeSlider.value += amount;
        private void SetLevelData(LevelModel data) => _levelData = data;

        private void Generate() => StartCoroutine(GenerateGridCells());

        IEnumerator GenerateGridCells()
        {
            yield return new WaitUntil(() => _levelData != null);

            _analytics.SendEvent("level_start",
                new Dictionary<string, object>() { { "level_index", _levelData.Level } });

            _gridController.GenerateInitialGidCell(_levelData);
        }
    }
}