using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GridController
{
    private AddScoreEventBus _AddScoreEventBus;
    private GenericEventBus _BlockDestructionEventBus;

    private PoolManager _poolManager;
    private UserInputManager _userInputManager;
    private TurnManager _turnManager;


    private GridModel _model;

    private GridInteractableChecker _gridInteractableChecker;
    private GridBlockCollapse _gridBlockCollapse;
    private GridBlockLifeCycle _gridBlockLifeCycle;
    private GenerateInitialGrid _initialGeneration;

    public GridController(GridModel model, AddScoreEventBus addScoreEventBus, GenericEventBus blockDestructionEventBus, PoolManager poolManager, UserInputManager userInputManager, TurnManager turnManager)
    {
        _AddScoreEventBus = addScoreEventBus;
        _BlockDestructionEventBus = blockDestructionEventBus;
        _poolManager = poolManager;
        _userInputManager = userInputManager;
        _turnManager = turnManager;

        _model = model;

        _initialGeneration =        new(_model, _poolManager);
        _gridInteractableChecker =  new(_model, _poolManager);
        _gridBlockLifeCycle =       new(_model, _poolManager, _gridInteractableChecker, _AddScoreEventBus);
        _gridBlockCollapse =        new(_model);
    }

    public async Task Interact(Vector2Int inputCoords, bool boostedInput)
    {
        if (_model.GridData.TryGetValue(inputCoords, out GridCellModel gridCell))
        {
            if (gridCell.BlockModel == null)
                return;

            if (!boostedInput)
                InteractionAtGridCell(gridCell);
            else
            {
                _gridBlockLifeCycle.DestroyBlockOnLaserBooster(gridCell);
                await Task.Delay(250);

                RegenerateGrid();
            }
        }
    }

    public void GenerateInitialGidCell(LevelModel levelModel)
        => _initialGeneration.Initialize(levelModel);


    private void InteractionAtGridCell(GridCellModel gridCell)
    {
        _userInputManager.BlockInputByGridInteraction(true);
        OpenCloseAutoclickSystem(gridCell).ManageTaskExeption();
    }

    private async Task OpenCloseAutoclickSystem(GridCellModel gridCell)
    {
        bool autoInput = false;

        _model.MatchOpenList.Add(gridCell);
        while (_model.MatchOpenList.Count > 0)
        {
            GridCellModel tappedGridCell = _model.MatchOpenList[0];
            _model.MatchOpenList.RemoveAt(0);
            InteractionCore(tappedGridCell, autoInput).ManageTaskExeption();

            _model.MatchClosedList.Clear();
            _gridBlockLifeCycle.BoosterGridCell = null;
            autoInput = true;
            await Task.Delay(500);
        }

        _model.MatchOpenList.Clear();
        _userInputManager.BlockInputByGridInteraction(false);
    }
    private async Task InteractionCore(GridCellModel gridCell, bool autoInput)
    {
        if (!CheckInteractionWith(gridCell))
            return;

        AddScoreOnInteractionSucceed();

        if (!autoInput)
            _turnManager.InteractionUsed();

        _gridBlockLifeCycle.DestroyBlocksOnActionSucceed();

        if(_gridBlockLifeCycle.BoosterGridCell == null)
            _gridBlockLifeCycle.SpawnBooster(gridCell.AnchorCoords);

        await Task.Delay(250);

        RegenerateGrid();
    }

    private void RegenerateGrid()
    {
        _gridBlockCollapse.CheckCollapseBoard();
        _gridBlockLifeCycle.GenerateBlocksOnEmptyCells();
        _gridInteractableChecker.CheckBoardPossible();
        _gridBlockLifeCycle.CheckTriggeredBoostersToInteract();
    }

    private bool CheckInteractionWith(GridCellModel gridCell)
    {
        bool boosterMatchInteraction = false;

        if (gridCell.BlockModel.Booster == null)
            OpenClosedListMatchCellsGetter(gridCell);
        else
        {
            CheckActionOnBoosterBased(gridCell);
            boosterMatchInteraction = true;
        }

        return _model.MatchClosedList.Count >= 2 || boosterMatchInteraction;
    }

    private void CheckActionOnBoosterBased(GridCellModel gridCell)
    {
        _gridBlockLifeCycle.BoosterGridCell = gridCell;
        gridCell.BlockModel.Booster.OnInteraction(gridCell.BlockModel.Coords, _model);
    }

    private void OpenClosedListMatchCellsGetter(GridCellModel touchedGridCell)
    {
        List<GridCellModel> _matchOpenList = new();

        if (touchedGridCell.BlockModel == null)
            return;

        _matchOpenList.Add(touchedGridCell);

        while (_matchOpenList.Count > 0)
        {
            GridCellModel selectedGridCell = _matchOpenList[0];
            _matchOpenList.RemoveAt(0);
            _model.MatchClosedList.Add(selectedGridCell);

            foreach (Vector2Int coords in selectedGridCell.BlockModel.Coords.GetCrossCoords())
            {
                if (_model.GridData.TryGetValue(coords, out GridCellModel objectiveCell) 
                    && objectiveCell.BlockModel != null && touchedGridCell.BlockModel.Id == objectiveCell.BlockModel.Id 
                    && !_matchOpenList.Contains(objectiveCell) && !_model.MatchClosedList.Contains(objectiveCell))
                {
                    _matchOpenList.Add(objectiveCell);
                }
            }
        }
    }

    private void AddScoreOnInteractionSucceed()
    {
        int elementCount = 0;
        int cellId = _model.MatchClosedList[0].BlockModel.Id;

        foreach (GridCellModel CellController in _model.MatchClosedList)
        {
            if (CellController.BlockModel.Booster == null)
            {
                cellId = CellController.BlockModel.Id;
                elementCount++;
            }
        }

        _BlockDestructionEventBus.NotifyEvent();

        if (_model.MatchClosedList[0].BlockModel.Booster != null)
            _AddScoreEventBus.NotifyEvent(cellId, elementCount);
    }
   
}