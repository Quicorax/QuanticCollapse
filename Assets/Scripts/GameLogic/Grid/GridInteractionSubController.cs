using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInteractionSubController : MonoBehaviour
{
    [SerializeField] 
    private AddScoreEventBus _AddScoreEventBus;
    [SerializeField] 
    private GenericEventBus _BlockDestructionEventBus;

    [SerializeField] 
    private GridController Controller; //TODO: this is not assigned
    [SerializeField] 
    private GridModel Model;           //TODO: this is not assigned

    [SerializeField] 
    private PoolManager _poolManager;
    [SerializeField] 
    private UserInputManager _userInputManager;
    [SerializeField] 
    private TurnManager _turnManager;

    private GridCellController _boosterGridCell;
    private BoostersLogic _boostersLogic = new();

    [HideInInspector]
    public List<GridCellController> MatchClosedList = new();
    private List<GridCellController> _autoclickOpenList = new();

    private int boostersInGrid;

    public void InteractionAtGrid(bool isRegularInput, GridCellController gridCell)
    {
        if (!gridCell.CheckHasBlock())
            return;

        if (isRegularInput)
            InteractionAtGridCell(gridCell);
        else
            LaserBlock(gridCell);
    }
    void LaserBlock(GridCellController gridCell)
    {
        if (gridCell.BlockModel.Booster == null)
            _AddScoreEventBus.NotifyEvent(gridCell.BlockModel.Id, 1);

        SingleBlockDestruction(gridCell);
        Invoke(nameof(RegenerateGrid), .25f);
    }
    void InteractionAtGridCell(GridCellController gridCell)
    {
        _userInputManager.BlockInputByGridInteraction(true);
        StartCoroutine(OpenCloseAutoclickSystem(gridCell));
    }

    IEnumerator OpenCloseAutoclickSystem(GridCellController gridCell)
    {
        bool autoInput = false;

        _autoclickOpenList.Add(gridCell);
        while (_autoclickOpenList.Count > 0)
        {
            GridCellController tappedGridCell = _autoclickOpenList[0];
            _autoclickOpenList.RemoveAt(0);
            InteractionCore(tappedGridCell, autoInput);

            MatchClosedList.Clear();
            _boosterGridCell = null;
            autoInput = true;
            yield return new WaitForSeconds(0.5f);
        }
  
        _autoclickOpenList.Clear();
        _userInputManager.BlockInputByGridInteraction(false);
    }
    void InteractionCore(GridCellController gridCell, bool autoInput)
    {
        if (!CheckInteractionWith(gridCell))
            return;

        AddScoreOnInteractionSucceed();

        if(!autoInput)
            _turnManager.InteractionUsed();

        DestroyBlocksOnActionSucceed();

        CheckForBoosterSpawnOnInteractionSucceed(gridCell.AnchorCoords);

        Invoke(nameof(RegenerateGrid), .25f);
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

        return MatchClosedList.Count >= 2 || boosterMatchInteraction;
    }

    void CheckActionOnBoosterBased(GridCellController gridCell)
    {
        _boosterGridCell = gridCell;
        gridCell.CallBoosterInteraction(gridCell.GetBlockCoords(), Controller);
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
            MatchClosedList.Add(selectedGridCell);

            foreach (Vector2Int coords in selectedGridCell.GetBlockCoords().GetCrossCoords())
            {
                if (Model.GridData.TryGetValue(coords, out GridCellController objectiveCell) && objectiveCell.CheckHasBlock() &&
                    touchedGridCell.GetBlockId() == objectiveCell.GetBlockId() && 
                    !_matchOpenList.Contains(objectiveCell) && !MatchClosedList.Contains(objectiveCell))
                {
                    _matchOpenList.Add(objectiveCell);
                }
            }
        }
    }

    void AddScoreOnInteractionSucceed()
    {
        int elementCount = 0;
        int matchId = MatchClosedList[0].GetBlockId();

        foreach (var CellController in MatchClosedList)
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

            _poolManager.DeSpawnBlockView(_boosterGridCell.GetBlockId(), Model.GridObjects[_boosterGridCell.AnchorCoords]);
            _boosterGridCell.RemoveBlock();
        }

        foreach (var dynamicBlock in MatchClosedList)
        {
            if (dynamicBlock.CheckIsBooster())
                dynamicBlock.SetIsTriggered(true);
            else
                SingleBlockDestruction(dynamicBlock);
        }
    }
    void SingleBlockDestruction(GridCellController dynamicBlock)
    {
        _poolManager.DeSpawnBlockView(dynamicBlock.GetBlockId(), Model.GridObjects[dynamicBlock.AnchorCoords]);
        dynamicBlock.RemoveBlock();
    }

    void CheckForBoosterSpawnOnInteractionSucceed(Vector2Int coords)
    {
        if (_boosterGridCell != null)
            return;

        if(_boostersLogic.CheckBaseBoosterSpawn(MatchClosedList.Count, out BaseBooster booster))
        {
            boostersInGrid++;

            Transform newBooster = _poolManager.SpawnBlockView(booster.BoosterKindId, coords).transform;

            newBooster.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            newBooster.DOPunchRotation(Vector3.forward * 120, 0.3f);

            Controller.FillGidCellWithBooster(coords, booster, newBooster.gameObject);
        }
    }

    void CheckCollapseBoard()
    {
        foreach (var element in Model.GridData)
        {
            if (element.Value.CheckHasBlock())
            {
               int cellCollapseSteps = 0;

                for (int y = element.Key.y; y >= 0; y--)
                {
                    if (Model.GridData.TryGetValue(new Vector2Int(element.Key.x, y), out GridCellController gridCell) 
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
        foreach (var gridCell in Model.GridData.Values)
        { 
            if(gridCell.CheckHasBlock() && gridCell.CheckCollapseSteps() > 0)
            {
                Vector2Int newCoords = gridCell.GetBlockCoords() + Vector2Int.down * gridCell.CheckCollapseSteps();

                GridCellController controller = Model.GridData[newCoords];
                controller.SetDynamicBlockOnCell(gridCell.GetModel());
                controller.SetCoords(newCoords);
                controller.SetCollapseSteps(0);
                gridCell.RemoveBlock();

                GameObject gridObject = Model.GridObjects[gridCell.AnchorCoords];
                gridObject.transform.DOMoveY(newCoords.y, 0.4f).SetEase(Ease.OutBounce);

                Model.GridObjects[newCoords] = gridObject;
            }
        }
    }

    void GenerateBlocksOnEmptyCells()
    {
        foreach (var item in Model.GridData)
        {
            if (!item.Value.CheckHasBlock())
                Controller.FillGidCell(item.Key);
        }

        if (!CheckImposibleBeard())
            NonInteractableBoard();
    }

    void CheckTriggeredBoostersToInteract()
    {
        foreach (var gridCell in Model.GridData.Values)
        {
            if (gridCell.CheckHasBlock() && gridCell.CheckIsTriggered() && !_autoclickOpenList.Contains(gridCell))
            {
                _autoclickOpenList.Add(gridCell);
            }
        }
    }

    void NonInteractableBoard()
    {
        int x = Random.Range(0, 9);
        int y = Random.Range(0, 7);

        Vector2Int randomCoords = new(x, y);

        if(Model.GridData.TryGetValue(randomCoords, out GridCellController cell))
        {
            _poolManager.DeSpawnBlockView(cell.GetBlockId(), Model.GridObjects[cell.AnchorCoords]);
            cell.RemoveBlock();
        }

        GameObject boosterObject = _poolManager.SpawnBlockView(4, cell.GetBlockCoords());
        Controller.FillGidCellWithBooster(cell.GetBlockCoords(), new BoosterRowColumn(100), boosterObject);
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
        foreach (var item in Model.GridData)
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
            if (Model.GridData.TryGetValue(coords, out GridCellController objectiveCell) &&
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