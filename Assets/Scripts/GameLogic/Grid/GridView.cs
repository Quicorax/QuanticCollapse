using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum ElementKind { Attack, Defense, Intel, Speed, BoosterRowColumn, BoosterBomb, BoosterKindBased };

[System.Serializable]
public struct ControllerElements
{
    public GenericEventBus _LoseConditionEventBus;
    public GenericEventBus _WinConditionEventBus;
    public GenericIntEventBus _enemyDamagedEventBus;
    public GenericIntEventBus _playerDamagedEventBus;

    public GridInteractionSubController _interactionsController;
    public PoolManager _poolManager;
}

public class GridView : MonoBehaviour
{
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    [SerializeField] private PlayerStarshipData playerData;
    [SerializeField] private EnemyStarshipData enemyData;


    [SerializeField] private Slider enemyLifeSlider;
    [SerializeField] private Slider playerLifeSlider;

    [SerializeField] private GenericIntEventBus _enemyDamagedEventBus;
    [SerializeField] private GenericIntEventBus _playerDamagedEventBus;

    public ControllerElements controllerElements;

    public GridController Controller;

    [SerializeField] private ExternalBoosterView _externalBoosterView;
    private LevelModel _levelData;

    private AnalyticsGameService _analytics;

    private void Awake()
    {
        _enemyDamagedEventBus.Event += EnemyDamaged;
        _playerDamagedEventBus.Event += PlayerDamaged;
        _LevelInjected.Event += SetLevelData;

        _analytics = ServiceLocator.GetService<AnalyticsGameService>();
    }
    private void OnDestroy()
    {
        _enemyDamagedEventBus.Event -= EnemyDamaged;
        _playerDamagedEventBus.Event -= PlayerDamaged;
        _LevelInjected.Event -= SetLevelData;
    }
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        Controller = new(controllerElements);
        _externalBoosterView.Initialize();

        GenerateGridCells();

        Controller.ModifyPlayerLife(playerData.starshipLife);
        Controller.ModifyEnemyLife(20);

    }
    public void ProcessInput(Vector2Int inputCoords, bool boostedInput) { Controller.ListenInput(inputCoords, boostedInput); }
    public void PlayerDamaged(int amount) => playerLifeSlider.value += amount; 
    public void EnemyDamaged(int amount) => enemyLifeSlider.value += amount;
    public void SetLevelData(LevelModel data) => _levelData = data;


    void GenerateGridCells()
    {
        _analytics.SendEvent("level_start",
            new Dictionary<string, object>() { { "level_index", _levelData.Level } });

        for (int x = 0; x < _levelData.LevelWidth; x++)
        {
            for (int y = 0; y < _levelData.LevelHeight; y++)
            {
                Controller.GenerateInitialGidCell(_levelData, new GridCellController(new(x, y)));
            }
        }
    }
}