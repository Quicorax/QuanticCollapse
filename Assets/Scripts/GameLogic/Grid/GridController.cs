using UnityEngine;

public class GridController
{
    public GridModel Model;

    public GridCommandProcessor CommandProcessor;
    public GridInteractionSubController InteractionsController;

    private GenericEventBus _LoseConditionEventBus;
    private GenericEventBus _WinConditionEventBus;
    private GenericIntEventBus _enemyDamagedEventBus;
    private GenericIntEventBus _playerDamagedEventBus;

    private PoolManager _poolManager;

    public GridController(ControllerElements elements)
    {
        Model = new();

        _LoseConditionEventBus = elements._LoseConditionEventBus;
        _WinConditionEventBus = elements._WinConditionEventBus;
        _enemyDamagedEventBus = elements._enemyDamagedEventBus;
        _playerDamagedEventBus = elements._playerDamagedEventBus;

        InteractionsController = elements._interactionsController;
        _poolManager = elements._poolManager;

        CommandProcessor = new(Model);
    }

    public void ListenInput(Vector2Int inputCoords, bool boostedInput)
    {
        if (!boostedInput)
            CommandProcessor.ProcessCommand(new UserInteractionCommand(InteractionsController, inputCoords));
        else
            CommandProcessor.ProcessCommand(new BlockLaserCommand(InteractionsController, inputCoords));
    }
    public void ModifyPlayerLife(int amount) { CommandProcessor.ProcessCommand(new ModifyPlayerLifeCommand(_LoseConditionEventBus, _playerDamagedEventBus, amount)); }
    public void ModifyEnemyLife(int amount) { CommandProcessor.ProcessCommand(new ModifyEnemyLifeCommand(_WinConditionEventBus, _enemyDamagedEventBus, amount)); }
    public void GenerateInitialGidCell(LevelModel levelModel, GridCellController cell) { CommandProcessor.ProcessCommand(new GenerateInitialGridCellCommand(_poolManager, levelModel, cell)); }
    public void FillGidCell(Vector2Int coordsToFill) { CommandProcessor.ProcessCommand(new FillGridCellCommand(_poolManager, coordsToFill)); }
    public void FillGidCellWithBooster(Vector2Int coordsToFill, GameObject boosterObject, BaseBooster baseBooster) { CommandProcessor.ProcessCommand(new FillGridCellWithBoosterCommand(baseBooster, coordsToFill, boosterObject)); }
}