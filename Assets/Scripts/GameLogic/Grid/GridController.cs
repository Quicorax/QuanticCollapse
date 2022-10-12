using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GridController
{
    private AddScoreEventBus _AddScoreEventBus;
    private GenericEventBus _BlockDestructionEventBus;

    private PoolManager _poolManager;
    private UserInputManager _userInputManager;
    private TurnManager _turnManager;

    private GridCellController _boosterGridCell;
    private BoostersLogic _boostersLogic = new();

    private GridModel _model;
    private GameConfigService _config;

    private int boostersInGrid;

    public GridController(GridModel model, AddScoreEventBus addScoreEventBus, GenericEventBus blockDestructionEventBus, PoolManager poolManager, UserInputManager userInputManager, TurnManager turnManager)
    {
        _AddScoreEventBus = addScoreEventBus;
        _BlockDestructionEventBus = blockDestructionEventBus;
        _poolManager = poolManager;
        _userInputManager = userInputManager;
        _turnManager = turnManager;

        _model = model;
        _config = ServiceLocator.GetService<GameConfigService>();
    }

    public void ListenInput(Vector2Int inputCoords, bool boostedInput)
    {
        if (!boostedInput)
        {
            if (_model.GridData.TryGetValue(inputCoords, out GridCellController gridCell))
            {
                InteractionAtGrid(true, gridCell);
            }
        }
        else
        {
            if (_model.GridData.TryGetValue(inputCoords, out GridCellController gridCell))
                InteractionAtGrid(false, gridCell);
        }
    }
    public void GenerateInitialGidCell(LevelModel levelModel, GridCellController cell)
    {
        var a = new GenerateInitialGridCellCommand(_poolManager, levelModel, cell);
        a.Do(_model);
    }
    public void InteractionAtGrid(bool isRegularInput, GridCellController gridCell)
    {
        if (!gridCell.CheckHasBlock())
            return;

        if (isRegularInput)
            InteractionAtGridCell(gridCell);
        else
            LaserBlock(gridCell).ManageTaskExeption();
    }
    async Task LaserBlock(GridCellController gridCell)
    {
        if (gridCell.BlockModel.Booster == null)
            _AddScoreEventBus.NotifyEvent(gridCell.BlockModel.Id, 1);

        SingleBlockDestruction(gridCell);
        await Task.Delay(250);
        RegenerateGrid();
    }
    void InteractionAtGridCell(GridCellController gridCell)
    {
        _userInputManager.BlockInputByGridInteraction(true);
        OpenCloseAutoclickSystem(gridCell).ManageTaskExeption();
    }

    async Task OpenCloseAutoclickSystem(GridCellController gridCell)
    {
        bool autoInput = false;

        _model.MatchOpenList.Add(gridCell);
        while (_model.MatchOpenList.Count > 0)
        {
            GridCellController tappedGridCell = _model.MatchOpenList[0];
            _model.MatchOpenList.RemoveAt(0);
            InteractionCore(tappedGridCell, autoInput).ManageTaskExeption();

            _model.MatchClosedList.Clear();
            _boosterGridCell = null;
            autoInput = true;
            await Task.Delay(500);
        }

        _model.MatchOpenList.Clear();
        _userInputManager.BlockInputByGridInteraction(false);
    }
    async Task InteractionCore(GridCellController gridCell, bool autoInput)
    {
        if (!CheckInteractionWith(gridCell))
            return;

        AddScoreOnInteractionSucceed();

        if(!autoInput)
            _turnManager.InteractionUsed();

        DestroyBlocksOnActionSucceed();

        CheckForBoosterSpawnOnInteractionSucceed(gridCell.AnchorCoords);

        await Task.Delay(250);
        RegenerateGrid();
    }

    void RegenerateGrid()
    {
        CheckCollapseBoard();

        GenerateBlocksOnEmptyCells();

        CheckTriggeredBoostersToInteract();
    }

    bool CheckInteractionWith(GridCellController gridCell)
    {
        bool boosterMatchInteraction = false;

        if (!gridCell.CheckIsBooster())
            OpenClosedListMatchCellsGetter(gridCell);
        else
        {
            CheckActionOnBoosterBased(gridCell);
            boosterMatchInteraction = true;
        }

        return _model.MatchClosedList.Count >= 2 || boosterMatchInteraction;
    }

    void CheckActionOnBoosterBased(GridCellController gridCell)
    {
        _boosterGridCell = gridCell;
        gridCell.CallBoosterInteraction(gridCell.GetBlockCoords(), _model);
    }

    void OpenClosedListMatchCellsGetter(GridCellController touchedGridCell)
    {
        List<GridCellController > _matchOpenList = new();

        if (!touchedGridCell.CheckHasBlock())
            return;

        _matchOpenList.Add(touchedGridCell);

        while (_matchOpenList.Count > 0)
        {
            GridCellController selectedGridCell = _matchOpenList[0];
            _matchOpenList.RemoveAt(0);
            _model.MatchClosedList.Add(selectedGridCell);

            foreach (Vector2Int coords in selectedGridCell.GetBlockCoords().GetCrossCoords())
            {
                if (_model.GridData.TryGetValue(coords, out GridCellController objectiveCell) && objectiveCell.CheckHasBlock() &&
                    touchedGridCell.GetBlockId() == objectiveCell.GetBlockId() && 
                    !_matchOpenList.Contains(objectiveCell) && !_model.MatchClosedList.Contains(objectiveCell))
                {
                    _matchOpenList.Add(objectiveCell);
                }
            }
        }
    }

    void AddScoreOnInteractionSucceed()
    {
        int elementCount = 0;
        int matchId = _model.MatchClosedList[0].GetBlockId();

        foreach (var CellController in _model.MatchClosedList)
        {
            if (!CellController.CheckIsBooster())
            {
                matchId = CellController.GetBlockId();
                elementCount++;
            }
        }

        _BlockDestructionEventBus.NotifyEvent();

        if(matchId != 4 && matchId != 5 && matchId != 6)
        {
            _AddScoreEventBus.NotifyEvent(matchId, elementCount);
        }
    }

    void DestroyBlocksOnActionSucceed()
    {
        if(_boosterGridCell != null)
        {
            boostersInGrid--;

            _poolManager.DeSpawnBlockView(_boosterGridCell.GetBlockId(), _model.GridObjects[_boosterGridCell.AnchorCoords]);
            _boosterGridCell.RemoveBlock();
        }

        foreach (var dynamicBlock in _model.MatchClosedList)
        {
            if (dynamicBlock.CheckIsBooster())
                dynamicBlock.SetIsTriggered(true);
            else
                SingleBlockDestruction(dynamicBlock);
        }
    }
    void SingleBlockDestruction(GridCellController dynamicBlock)
    {
        _poolManager.DeSpawnBlockView(dynamicBlock.GetBlockId(), _model.GridObjects[dynamicBlock.AnchorCoords]);
        dynamicBlock.RemoveBlock();
    }

    void CheckForBoosterSpawnOnInteractionSucceed(Vector2Int coords)
    {
        if (_boosterGridCell != null)
            return;

        if(_boostersLogic.CheckBaseBoosterSpawn(_model.MatchClosedList.Count, out BaseBooster booster))
        {
            boostersInGrid++;

            Transform newBooster = _poolManager.SpawnBlockView(booster.BoosterKindId, coords).transform;

            newBooster.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            newBooster.DOPunchRotation(Vector3.forward * 120, 0.3f);

            _model.GridData[coords].BlockModel = new CellBlockModel(booster.BoosterKindId, coords, booster);
            _model.GridObjects[coords] = newBooster.gameObject;
        }
    }

    void CheckCollapseBoard()
    {
        foreach (var element in _model.GridData)
        {
            if (element.Value.CheckHasBlock())
            {
               int cellCollapseSteps = 0;

                for (int y = element.Key.y; y >= 0; y--)
                {
                    if (_model.GridData.TryGetValue(new Vector2Int(element.Key.x, y), out GridCellController gridCell) 
                        && !gridCell.CheckHasBlock())
                    {
                        cellCollapseSteps++;
                    }
                }

                element.Value.SetCollapseSteps(cellCollapseSteps);
            }
        }
        CollapseBlocks();
    }

    void CollapseBlocks()
    {
        foreach (var gridCell in _model.GridData.Values)
        { 
            if(gridCell.CheckHasBlock() && gridCell.CheckCollapseSteps() > 0)
            {
                Vector2Int newCoords = gridCell.GetBlockCoords() + Vector2Int.down * gridCell.CheckCollapseSteps();

                GridCellController controller = _model.GridData[newCoords];
                controller.SetDynamicBlockOnCell(gridCell.GetModel());
                controller.SetCoords(newCoords);
                controller.SetCollapseSteps(0);
                gridCell.RemoveBlock();

                GameObject gridObject = _model.GridObjects[gridCell.AnchorCoords];
                gridObject.transform.DOMoveY(newCoords.y, 0.4f).SetEase(Ease.OutBounce);

                _model.GridObjects[newCoords] = gridObject;
            }
        }
    }

    void GenerateBlocksOnEmptyCells()
    {
        foreach (var item in _model.GridData)
        {
            if (!item.Value.CheckHasBlock())
            {
                int _blockId = _config.GridBlocks.BaseBlocks[Random.Range(0, _config.GridBlocks.BaseBlocks.Count())].Id;
                GameObject newBlockView = _poolManager.SpawnBlockView(_blockId, new Vector2Int(item.Key.x, 8));
                newBlockView.transform.DOMoveY(item.Key.y, 0.4f).SetEase(Ease.OutBounce);

                _model.GridData[item.Key].SetDynamicBlockOnCell(new CellBlockModel(_blockId, item.Key));
                _model.GridObjects[item.Key] = newBlockView;
            }
        }

        if (!CheckImposibleBeard())
            NonInteractableBoard();
    }

    void CheckTriggeredBoostersToInteract()
    {
        foreach (var gridCell in _model.GridData.Values)
        {
            if (gridCell.CheckHasBlock() && gridCell.CheckIsTriggered() && !_model.MatchOpenList.Contains(gridCell))
            {
                _model.MatchOpenList.Add(gridCell);
            }
        }
    }

    void NonInteractableBoard()
    {
        int x = Random.Range(0, 9);
        int y = Random.Range(0, 7);

        Vector2Int randomCoords = new(x, y);

        if(_model.GridData.TryGetValue(randomCoords, out GridCellController cell))
        {
            _poolManager.DeSpawnBlockView(cell.GetBlockId(), _model.GridObjects[cell.AnchorCoords]);
            cell.RemoveBlock();
        }

        BaseBooster boosterLogic = new BoosterRowColumn(100);
        GameObject boosterObject = _poolManager.SpawnBlockView(boosterLogic.BoosterKindId, cell.GetBlockCoords());

        _model.GridData[cell.GetBlockCoords()].BlockModel = new CellBlockModel(boosterLogic.BoosterKindId, cell.GetBlockCoords(), boosterLogic);
        _model.GridObjects[cell.GetBlockCoords()] = boosterObject;

    }

    #region Board Interactable Checking 
    bool CheckImposibleBeard()
    {
        if (boostersInGrid > 0)
            return true;

        return SimulateInput();
    }

    bool SimulateInput()
    {
        foreach (var item in _model.GridData)
        {
            if (SimulateCheckInteractionWith(item.Value))
                return true;
        }

        return false;
    }
    bool SimulateCheckInteractionWith(GridCellController gridCell)
    {
        foreach (Vector2Int coords in gridCell.GetBlockCoords().GetCrossCoords())
        {
            if (_model.GridData.TryGetValue(coords, out GridCellController objectiveCell) &&
                gridCell.CheckHasBlock() && objectiveCell.CheckHasBlock() &&
                gridCell.GetBlockId() == objectiveCell.GetBlockId())
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}