using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public struct ControllerElements
{
    public GenericEventBus _LoseConditionEventBus;
    public GenericEventBus _WinConditionEventBus;
    public GenericIntEventBus _enemyDamagedEventBus;
    public GenericIntEventBus _playerDamagedEventBus;

    public GridController _interactionsController;
}

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
        GridModel = new();

        Initialize();
    }
    public void Initialize()
    {
        GridController.Initialize(GridModel);

        ExternalBoosterView externalBoosterView = new();
        externalBoosterView.Initialize(_ScreenEffects, GridModel, externalBoosterParent);

        ModifyEnemyLifeCommand dmgEnemy = new(_WinConfitionEventBus, _EnemyDamagedEventBus, 20);
        dmgEnemy.Do(GridModel);
        ModifyPlayerLifeCommand dmgPlayer = new(_WinConfitionEventBus, _PlayerDamagedEventBus, 20);
        dmgPlayer.Do(GridModel);
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