using UnityEngine;
using UnityEngine.UI;

public class GridCommandHall
{
    public GridCommandProcessor CommandProcessor;

    public GridCommandHall(GridCommandProcessor commandProcessor)
    {
        CommandProcessor = commandProcessor;
    }

    public void CommandListenInput(Vector2Int inputCoords, bool boostedInput, GridInteractionsController _interactionsController)
    {
        if (!boostedInput)
            CommandProcessor.ProcessCommand(new UserInteractionCommand(_interactionsController, inputCoords));
        else
            CommandProcessor.ProcessCommand(new BlockLaserCommand(_interactionsController, inputCoords));
    }
    public void CommandModifyPlayerLife(int amount, GenericEventBus _LoseConditionEventBus, Slider playerLifeView)
    {
        CommandProcessor.ProcessCommand(new ModifyPlayerLifeCommand(_LoseConditionEventBus,amount, playerLifeView));
    }
    public void CommandModifyEnemyLife(int amount, GenericEventBus _WinConditionEventBus, Slider enemyLifeView)
    {
        CommandProcessor.ProcessCommand(new ModifyEnemyLifeCommand(_WinConditionEventBus, amount, enemyLifeView));
    }
    public void CommandGenerateInitialGidCell(PoolManager _poolManager, LevelGridData levelData, GridCellController cell, Vector2Int cellCoords)
    {
        CommandProcessor.ProcessCommand(new GenerateInitialGridCellCommand(_poolManager, levelData.gridInitialLayout, cell, cellCoords));
    }
    public void CommandFillGidCell(Vector2Int coordsToFill, PoolManager _poolManager)
    {
        CommandProcessor.ProcessCommand(new FillGridCellCommand(_poolManager, coordsToFill));
    }
    public void CommandFillGidCellWithBooster(Vector2Int coordsToFill, GameObject boosterObject, BaseBooster baseBooster)
    {
        CommandProcessor.ProcessCommand(new FillGridCellWithBoosterCommand(baseBooster, coordsToFill, boosterObject));
    }
}
