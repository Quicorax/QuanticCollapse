using UnityEngine;
using UnityEngine.UI;

public class VirtualGridView : MonoBehaviour
{
    [SerializeField]
    private TapOnCoordsEventBus _TapOnCoordsEventBus;

    private VirtualGridController Controller = new VirtualGridController();

    private GridInteractionsController _interactionsController;
    [SerializeField] private PoolManager _poolManager;

    public Slider playerLifeView;
    public Slider enemyLifeView;

    private void Awake()
    {
        _interactionsController = GetComponent<GridInteractionsController>();
        
        _TapOnCoordsEventBus.Event += ListenInput;
    }
    private void OnDestroy()
    {
        _TapOnCoordsEventBus.Event -= ListenInput;
    }

    public void ListenInput(Vector2 inputCoords, bool boostedInput)
    {
        Controller.ProcessCommand(new UserInteractionCommand(this, _interactionsController, inputCoords, boostedInput));

    }

    #region Level Meta Life 
    public void ModifyPlayerLife(int amount)
    {
        Controller.ProcessCommand(new ModifyPlayerLifeCommand(amount, playerLifeView));
    }
    public void ModifyEnemyLife(int amount)
    {
        Controller.ProcessCommand(new ModifyEnemyLifeCommand(amount, enemyLifeView));
    }
    #endregion

    #region Generation
    public void GenerateGidCell(Vector2 cellCoords, GridCell cell)
    {
        Controller.ProcessCommand(new GenerateGridCellCommand(cellCoords, cell));
    }
    public void FillGidCellWithInitialDisposition(Vector2 coordsToFill, LevelGridData levelData)
    {
        Controller.ProcessCommand(new FillGidCellWithInitialDispositionCommand(_poolManager, coordsToFill, levelData.gridInitialLayout));
    }
    public void FillGidCell(Vector2 coordsToFill)
    {
        Controller.ProcessCommand(new FillGridCellCommand(_poolManager, coordsToFill));
    }
    public void FillGidCellWithBooster(Vector2 coordsToFill, GameObject boosterObject, BaseBooster baseBooster)
    {
        Controller.ProcessCommand(new FillGridCellWithBoosterCommand(baseBooster, coordsToFill, boosterObject));
    }
    #endregion
}
