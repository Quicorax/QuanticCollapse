using UnityEngine;
using UnityEngine.UI;

public class VirtualGridController : MonoBehaviour
{
    public VirtualGridModel GridModel = new();

    public GridCommandProcessor CommandProcessor;
    public GridCommandHall CommandHall;

    [SerializeField] private TapOnCoordsEventBus _TapOnCoordsEventBus;
    [SerializeField] private GenericEventBus _LoseConditionEventBus;
    [SerializeField] private GenericEventBus _WinConditionEventBus;

    [SerializeField] private GridInteractionsController _interactionsController;
    [SerializeField] private PoolManager _poolManager;

    [SerializeField] private Slider playerLifeView;
    [SerializeField] private Slider enemyLifeView;


    private void Awake()
    {
        _TapOnCoordsEventBus.Event += ListenInput;

        CommandProcessor = new(GridModel);
        CommandHall = new(CommandProcessor);
    }
    private void OnDisable()
    {
        _TapOnCoordsEventBus.Event -= ListenInput;
    }

    public void ListenInput(Vector2Int inputCoords, bool boostedInput) { CommandHall.CommandListenInput(inputCoords, boostedInput, _interactionsController); }
    public void ModifyPlayerLife(int amount) { CommandHall.CommandModifyPlayerLife(amount, _LoseConditionEventBus, playerLifeView); }
    public void ModifyEnemyLife(int amount) { CommandHall.CommandModifyEnemyLife(amount, _WinConditionEventBus, enemyLifeView); }
    public void GenerateInitialGidCell(Vector2Int cellCoords, LevelGridData levelData, GridCellController cell) { CommandHall.CommandGenerateInitialGidCell(_poolManager, levelData, cell, cellCoords); }
    public void FillGidCell(Vector2Int coordsToFill) { CommandHall.CommandFillGidCell(coordsToFill, _poolManager); }
    public void FillGidCellWithBooster(Vector2Int coordsToFill, GameObject boosterObject, BaseBooster baseBooster) { CommandHall.CommandFillGidCellWithBooster(coordsToFill, boosterObject, baseBooster); }
}
