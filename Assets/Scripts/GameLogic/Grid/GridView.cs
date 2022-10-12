using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GridView : MonoBehaviour
{
    [SerializeField] 
    private ExternalBoosterScreenEffectEventBus _ScreenEffects;
    [SerializeField] 
    private LevelInjectedEventBus _LevelInjected;
    [SerializeField]
    private GenericEventBus _PoolLoaded;
    [SerializeField]
    private GenericEventBus _WinConfitionEventBus;
    [SerializeField] 
    private GenericIntEventBus _EnemyDamagedEventBus;
    [SerializeField] 
    private GenericIntEventBus _PlayerDamagedEventBus;
    [SerializeField] 
    private AddScoreEventBus _AddScoreEventBus;
    [SerializeField] 
    private GenericEventBus _BlockDestructionEventBus;

    [SerializeField] 
    private PoolManager _poolManager;
    [SerializeField] 
    private UserInputManager _userInputManager;
    [SerializeField] 
    private TurnManager _turnManager;

    [SerializeField] 
    private PlayerStarshipData playerData;
    [SerializeField] 
    private EnemyStarshipData enemyData;
    [SerializeField]
    private Transform externalBoosterParent;

    [SerializeField] 
    private Slider enemyLifeSlider;
    [SerializeField] 
    private Slider playerLifeSlider;
    
    public GridController GridController; //TODO: This should be private
    public GridModel GridModel;           //TODO: This should be private

    private LevelModel _levelData;

    private AnalyticsGameService _analytics;

    private void Awake()
    {
        _EnemyDamagedEventBus.Event += EnemyDamaged;
        _PlayerDamagedEventBus.Event += PlayerDamaged;
        _LevelInjected.Event += SetLevelData;
        _PoolLoaded.Event += Generater;

        _analytics = ServiceLocator.GetService<AnalyticsGameService>();

        GridModel = new();
        GridController = new(GridModel, _AddScoreEventBus, _BlockDestructionEventBus, _poolManager, _userInputManager, _turnManager);
    }
    private void OnDestroy()
    {
        _EnemyDamagedEventBus.Event -= EnemyDamaged;
        _PlayerDamagedEventBus.Event -= PlayerDamaged;
        _LevelInjected.Event -= SetLevelData;
        _PoolLoaded.Event -= Generater;
    }
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        ExternalBoosterView externalBoosterView = new();
        externalBoosterView.Initialize(_ScreenEffects, GridModel, externalBoosterParent);

        ModifyEnemyHealth.Initialize(GridModel, _WinConfitionEventBus, _EnemyDamagedEventBus);
        ModifyEnemyHealth.Do(20); //TODO: Set initial Health based on player data

        ModifyPlayerHealth.Initialize(GridModel, _WinConfitionEventBus, _PlayerDamagedEventBus);
        ModifyPlayerHealth.Do(20); //TODO: Set initial Health based on enemy data
    }
    public void ProcessInput(Vector2Int inputCoords, bool boostedInput) 
        => GridController.ListenInput(inputCoords, boostedInput); 
    public void PlayerDamaged(int amount) => playerLifeSlider.value += amount; 
    public void EnemyDamaged(int amount) => enemyLifeSlider.value += amount;
    public void SetLevelData(LevelModel data) => _levelData = data;

    private void Generater() => StartCoroutine(GenerateGridCells());

    IEnumerator GenerateGridCells()
    {
        yield return new WaitUntil(()=> _levelData != null);

        _analytics.SendEvent("level_start",
            new Dictionary<string, object>() { { "level_index", _levelData.Level } });

        for (int x = 0; x < _levelData.LevelWidth; x++)
        {
            for (int y = 0; y < _levelData.LevelHeight; y++)
            {
                GridController.GenerateInitialGidCell(_levelData, new GridCellController(new(x, y)));
            }
        }
    }
}