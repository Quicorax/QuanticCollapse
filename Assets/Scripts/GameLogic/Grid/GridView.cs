using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public struct ControllerElements
{
    public GenericEventBus _LoseConditionEventBus;
    public GenericEventBus _WinConditionEventBus;
    public GenericIntEventBus _enemyDamagedEventBus;
    public GenericIntEventBus _playerDamagedEventBus;

    public GridInteractionSubController _interactionsController;
}

public class GridView : MonoBehaviour
{
    [SerializeField] 
    private ExternalBoosterScreenEffectEventBus _ScreenEffects;
    [SerializeField] 
    private LevelInjectedEventBus _LevelInjected;

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
    
    [SerializeField] 
    private GenericIntEventBus _enemyDamagedEventBus;
    [SerializeField] 
    private GenericIntEventBus _playerDamagedEventBus;

    public ControllerElements controllerElements; //TODO: Remove this references

    public GridController Controller; //TODO: this is not a real controller

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
        Controller = new(controllerElements, GetComponent<PoolManager>());
        
        ExternalBoosterView externalBoosterView = new();
        externalBoosterView.Initialize(_ScreenEffects, Controller, externalBoosterParent);
        
        GenerateGridCells();
        
        Controller.ModifyPlayerLife(playerData.starshipLife);
        Controller.ModifyEnemyLife(20);

    }
    public void ProcessInput(Vector2Int inputCoords, bool boostedInput) => Controller.ListenInput(inputCoords, boostedInput); 
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