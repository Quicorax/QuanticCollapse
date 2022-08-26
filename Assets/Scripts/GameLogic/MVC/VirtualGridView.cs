using UnityEngine;
using UnityEngine.UI;

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

public class VirtualGridView : MonoBehaviour
{
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    [SerializeField] private PlayerStarshipData playerData;
    [SerializeField] private EnemyStarshipData enemyData;


    [SerializeField] private Slider enemyLifeSlider;
    [SerializeField] private Slider playerLifeSlider;

    [SerializeField] private GenericIntEventBus _enemyDamagedEventBus;
    [SerializeField] private GenericIntEventBus _playerDamagedEventBus;

    public ControllerElements controllerElements;

    public VirtualGridController Controller;
    private LevelGridData _levelData;

    private void Awake()
    {
        Controller = new(controllerElements);

        _enemyDamagedEventBus.Event += EnemyDamaged;
        _playerDamagedEventBus.Event += PlayerDamaged;
        _LevelInjected.Event += SetLevelData;

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
        GenerateGridCells();

        Controller.ModifyPlayerLife(playerData.starshipLife);
        Controller.ModifyEnemyLife(_levelData.EnemyStarshipMaxLife);
    }
    public void ProcessInput(Vector2Int inputCoords, bool boostedInput) { Controller.ListenInput(inputCoords, boostedInput); }
    void PlayerDamaged(int amount) => playerLifeSlider.value += amount; 
    void EnemyDamaged(int amount) => enemyLifeSlider.value += amount; 
    void SetLevelData(LevelGridData data) => _levelData = data; 
    void GenerateGridCells()
    {
        for (int x = 0; x < _levelData.GridInitialLayout.width; x++)
        {
            for (int y = 0; y < _levelData.GridInitialLayout.height; y++)
            {
                Vector2Int gridCellCoords = new(x, y);

                GridCellController newGridCell = new(gridCellCoords);
                Controller.GenerateInitialGidCell(gridCellCoords, _levelData, newGridCell);
            }
        }
    }
}
