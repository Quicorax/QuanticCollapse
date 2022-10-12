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

    private GridCellModel _boosterGridCell;
    private BoostersLogic _boostersLogic = new();

    private GridModel _model;
    private GameConfigService _config;

    private GridInteractableChecker _interactableGridChecker;

    public GridController(GridModel model, AddScoreEventBus addScoreEventBus, GenericEventBus blockDestructionEventBus, PoolManager poolManager, UserInputManager userInputManager, TurnManager turnManager)
    {
        _AddScoreEventBus = addScoreEventBus;
        _BlockDestructionEventBus = blockDestructionEventBus;
        _poolManager = poolManager;
        _userInputManager = userInputManager;
        _turnManager = turnManager;

        _model = model;
        _config = ServiceLocator.GetService<GameConfigService>();

        _interactableGridChecker = new(_model,_poolManager);
    }

    public void Interact(Vector2Int inputCoords, bool boostedInput)
    {
        if (_model.GridData.TryGetValue(inputCoords, out GridCellModel gridCell))
        {
            if (gridCell.BlockModel == null)
                return;

            if (!boostedInput)
                InteractionAtGridCell(gridCell);
            else
                LaserBlock(gridCell).ManageTaskExeption();
        }
    }
    public void GenerateInitialGidCell(LevelModel levelModel, GridCellModel cell)
    {
        GenerateInitialGrid InitialGeneration = new(_poolManager, levelModel, cell);
        InitialGeneration.Do(_model);
    }

    async Task LaserBlock(GridCellModel gridCell)
    {
        if (gridCell.BlockModel.Booster == null)
            _AddScoreEventBus.NotifyEvent(gridCell.BlockModel.Id, 1);

        _poolManager.DeSpawnBlockView(gridCell.BlockModel.Id, _model.GridObjects[gridCell.AnchorCoords]);
        gridCell.BlockModel = null;
        await Task.Delay(250);
        RegenerateGrid();
    }
    void InteractionAtGridCell(GridCellModel gridCell)
    {
        _userInputManager.BlockInputByGridInteraction(true);
        OpenCloseAutoclickSystem(gridCell).ManageTaskExeption();
    }

    async Task OpenCloseAutoclickSystem(GridCellModel gridCell)
    {
        bool autoInput = false;

        _model.MatchOpenList.Add(gridCell);
        while (_model.MatchOpenList.Count > 0)
        {
            GridCellModel tappedGridCell = _model.MatchOpenList[0];
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
    async Task InteractionCore(GridCellModel gridCell, bool autoInput)
    {
        if (!CheckInteractionWith(gridCell))
            return;

        AddScoreOnInteractionSucceed();

        if (!autoInput)
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

    bool CheckInteractionWith(GridCellModel gridCell)
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

    void CheckActionOnBoosterBased(GridCellModel gridCell)
    {
        _boosterGridCell = gridCell;
        gridCell.BlockModel.Booster.OnInteraction(gridCell.BlockModel.Coords, _model);
    }

    void OpenClosedListMatchCellsGetter(GridCellModel touchedGridCell)
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

    void AddScoreOnInteractionSucceed()
    {
        int elementCount = 0;
        int matchId = _model.MatchClosedList[0].BlockModel.Id;

        foreach (GridCellModel CellController in _model.MatchClosedList)
        {
            if (CellController.BlockModel.Booster == null)
            {
                matchId = CellController.BlockModel.Id;
                elementCount++;
            }
        }

        _BlockDestructionEventBus.NotifyEvent();

        if (matchId != 4 && matchId != 5 && matchId != 6)
            _AddScoreEventBus.NotifyEvent(matchId, elementCount);
    }

    void DestroyBlocksOnActionSucceed()
    {
        if (_boosterGridCell != null)
        {
            _interactableGridChecker.BoostersInGrid--;

            _poolManager.DeSpawnBlockView(_boosterGridCell.BlockModel.Id, _model.GridObjects[_boosterGridCell.AnchorCoords]);
            _boosterGridCell.BlockModel = null;
        }

        foreach (GridCellModel dynamicBlock in _model.MatchClosedList)
        {
            if (dynamicBlock.BlockModel.Booster != null)
                dynamicBlock.BlockModel.IsTriggered = true;
            else
            {
                _poolManager.DeSpawnBlockView(dynamicBlock.BlockModel.Id, _model.GridObjects[dynamicBlock.AnchorCoords]);
                dynamicBlock.BlockModel = null;
            }
        }
    }

    void CheckForBoosterSpawnOnInteractionSucceed(Vector2Int coords)
    {
        if (_boosterGridCell != null)
            return;

        if (_boostersLogic.CheckBaseBoosterSpawn(_model.MatchClosedList.Count, out BaseBooster booster))
        {
            _interactableGridChecker.BoostersInGrid++;

            Transform newBooster = _poolManager.SpawnBlockView(booster.BoosterKindId, coords).transform;

            newBooster.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            newBooster.DOPunchRotation(Vector3.forward * 120, 0.3f);

            _model.GridData[coords].BlockModel = new(booster.BoosterKindId, coords, booster);
            _model.GridObjects[coords] = newBooster.gameObject;
        }
    }

    void CheckCollapseBoard()
    {
        foreach (var element in _model.GridData)
        {
            if (element.Value.BlockModel != null)
            {
                int cellCollapseSteps = 0;

                for (int y = element.Key.y; y >= 0; y--)
                {
                    if (_model.GridData.TryGetValue(new(element.Key.x, y), out GridCellModel gridCell)
                        && gridCell.BlockModel == null)
                    {
                        cellCollapseSteps++;
                    }
                }
                element.Value.BlockModel.CollapseSteps = cellCollapseSteps;
            }
        }
        CollapseBlocks();
    }

    void CollapseBlocks()
    {
        foreach (var gridCell in _model.GridData.Values)
        {
            if (gridCell.BlockModel != null && gridCell.BlockModel.CollapseSteps > 0)
            {
                Vector2Int newCoords = gridCell.BlockModel.Coords + Vector2Int.down * gridCell.BlockModel.CollapseSteps;

                GridCellModel model = _model.GridData[newCoords];
                model.BlockModel = gridCell.BlockModel;
                model.BlockModel.Coords = newCoords;
                model.BlockModel.CollapseSteps = 0;
                gridCell.BlockModel = null;

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
            if (item.Value.BlockModel == null)
            {
                int _blockId = _config.GridBlocks.BaseBlocks[Random.Range(0, _config.GridBlocks.BaseBlocks.Count())].Id;
                GameObject newBlockView = _poolManager.SpawnBlockView(_blockId, new Vector2Int(item.Key.x, 8));
                newBlockView.transform.DOMoveY(item.Key.y, 0.4f).SetEase(Ease.OutBounce);

                _model.GridData[item.Key].BlockModel = new(_blockId, item.Key);
                _model.GridObjects[item.Key] = newBlockView;
            }
        }
        _interactableGridChecker.CheckBoardPossible();
 
    }

    void CheckTriggeredBoostersToInteract()
    {
        foreach (var gridCell in _model.GridData.Values)
        {
            if (gridCell.BlockModel != null && gridCell.BlockModel.IsTriggered && !_model.MatchOpenList.Contains(gridCell))
                _model.MatchOpenList.Add(gridCell);
        }
    }
}
