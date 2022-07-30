using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInteractionsController : MonoBehaviour
{
    private VirtualGridView _View;
    private VirtualGridModel _Model;

    [SerializeField] private AddScoreEventBus _AddScoreEventBus;

    [SerializeField] private BoostersLogic _boostersLogic;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private TurnManager _turnManager;

    bool generationComplete;

    int boostersInGrid;
    public bool isGridPossible;

    public void InteractionAtGridCell(GridCell gridCell, VirtualGridView View = null, VirtualGridModel Model = null, bool boostedInput = false)
    {
        if (_Model == null)
            _Model = Model;
        if (_View == null)
            _View = View;

        if (gridCell.blockInCell == null)
            return;

        StartCoroutine(InteractionCore(gridCell, boostedInput));

        _Model.matchList.Clear();
        _Model.boosterGridCell = null;
    }

    IEnumerator InteractionCore(GridCell gridCell, bool boostedInput)
    {
        if (CheckInteractionWith(gridCell, boostedInput))
        {
            generationComplete = false;

            if(!gridCell.blockInCell.isTriggered)
                _turnManager.InteractionUsed();

            AddScoreOnInteractionSucceed();

            DestroyBlocksOnActionSucceed();

            CheckForBoosterSpawnOnInteractionSucceed(gridCell.blockAnchorCoords);

            yield return new WaitForSeconds(0.25f);

            CheckCollapseBoard();

            GenerateBlocksOnEmptyCells();

            StartCoroutine(CheckTriggeredBoostersToInteract());
        }
    }


    bool CheckInteractionWith(GridCell gridCell, bool boostedInteraction)
    {
        bool boosterInteraction = false;

        if (boostedInteraction)
        {
            _Model.matchList.Add(gridCell);
            return true;
        }
        else
        {
            if (!gridCell.blockInCell.isBooster)
            {
                OpenClosedListMatchCellsGetter(gridCell);
            }
            else
            {
                CheckActionOnBoosterBased(gridCell);
                boosterInteraction = true;
            }
        }

        return _Model.matchList.Count >= 2 || boosterInteraction;
    }

    void CheckActionOnBoosterBased(GridCell gridCell)
    {
        _Model.boosterGridCell = gridCell;
        gridCell.blockInCell.booster.OnInteraction(gridCell.blockAnchorCoords, _Model);
    }

    void OpenClosedListMatchCellsGetter(GridCell touchedGridCell)
    {
        List<GridCell> closedList = new();
        List<GridCell> openList = new();

        if (!touchedGridCell.hasBlock)
            return;

        openList.Add(touchedGridCell);

        while (openList.Count > 0)
        {
            GridCell selectedGridCell = openList[0];
            openList.RemoveAt(0);
            closedList.Add(selectedGridCell);

            foreach (Vector2 coords in selectedGridCell.blockAnchorCoords.GetCrossCoords())
            {
                if (_Model.virtualGrid.TryGetValue(coords, out GridCell objectiveCell) && objectiveCell.hasBlock &&
                    touchedGridCell.blockInCell.blockKind == objectiveCell.blockInCell.blockKind && 
                    !openList.Contains(objectiveCell) && !closedList.Contains(objectiveCell))
                {
                    openList.Add(objectiveCell);
                }
            }
        }

        _Model.matchList = closedList;
    }

    void AddScoreOnInteractionSucceed()
    {
        foreach (var item in _Model.matchList)
        {
            if (!_Model.virtualGrid[item.blockAnchorCoords].blockInCell.isBooster)
            {
                _AddScoreEventBus.NotifyEvent(_Model.virtualGrid[item.blockAnchorCoords].blockInCell.blockKind, 1);
            }
        }
    }

    void DestroyBlocksOnActionSucceed()
    {
        if(_Model.boosterGridCell != null)
        {
            _Model.boosterGridCell.blockInCell.blockView.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                boostersInGrid--;

                _poolManager.DeSpawnBlockView(_Model.boosterGridCell.blockInCell.blockKind, _Model.boosterGridCell.blockInCell.blockView);
            });

            _Model.boosterGridCell.ResetGridCell();
        }

        foreach (var dynamicBlock in _Model.matchList)
        {
            if (dynamicBlock.blockInCell.isBooster)
            {
                dynamicBlock.blockInCell.isTriggered = true;
            }
            else
            {
                _poolManager.DeSpawnBlockView(dynamicBlock.blockInCell.blockKind, dynamicBlock.blockInCell.blockView);
                dynamicBlock.ResetGridCell();
            }
        }
    }

    void CheckForBoosterSpawnOnInteractionSucceed(Vector2 coords)
    {
        if (_Model.boosterGridCell != null)
            return;

        if(_boostersLogic.CheckBaseBoosterSpawn(_Model.matchList.Count, out BaseBooster booster))
        {
            boostersInGrid++;

            Transform newBooster = _poolManager.SpawnBlockView(booster.boosterKind, coords).transform;

            newBooster.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            newBooster.DOPunchRotation(Vector3.forward * 120, 0.3f);

            _View.FillGidCellWithBooster(coords, newBooster.gameObject, booster);
        }
    }

    void CheckCollapseBoard()
    {
        foreach (var element in _Model.virtualGrid)
        {
            if (element.Value.hasBlock)
            {
               int cellCollapseSteps = 0;

                for (int y = (int)element.Key.y; y >= 0; y--)
                {
                    if (_Model.virtualGrid.TryGetValue(new Vector2(element.Key.x, y), out GridCell gridCell) && !gridCell.hasBlock)
                    {
                        cellCollapseSteps++;
                    }
                }

                element.Value.blockInCell.collapseSteps = cellCollapseSteps;
            }
        }

        CollapseBlocks();
    }

    void CollapseBlocks()
    {
        foreach (var gridCell in _Model.virtualGrid.Values)
        { 
            if(gridCell.hasBlock && gridCell.blockInCell.collapseSteps > 0)
            {
                Vector2 newCoords = gridCell.blockInCell.blockCoords + Vector2.down * gridCell.blockInCell.collapseSteps;
                DynamicBlock dynamicBlock = gridCell.blockInCell;

                dynamicBlock.blockCoords = newCoords;
                dynamicBlock.collapseSteps = 0;

                dynamicBlock.blockView.transform.DOMoveY(newCoords.y, 0.4f).SetEase(Ease.OutBounce);

                _Model.virtualGrid[newCoords].SetDynamicBlockOnCell(dynamicBlock);
                gridCell.ResetGridCell();
            }
        }
    }

    void GenerateBlocksOnEmptyCells()
    {
        foreach (var item in _Model.virtualGrid)
        {
            if (!item.Value.hasBlock)
            {
                _View.FillGidCell(item.Key);
            }
        }

        generationComplete = true;

        isGridPossible = CheckImposibleBeard();
        if (!isGridPossible)
        {
            NonInteractableBoard();
        }
    }

    IEnumerator CheckTriggeredBoostersToInteract()
    {
        foreach (var item in _Model.virtualGrid)
        {
            if (item.Value.blockInCell != null && item.Value.blockInCell.isTriggered)
            {
                yield return new WaitUntil(()=> generationComplete);
                yield return new WaitForSeconds(0.5f);

                InteractionAtGridCell(item.Value);
            }
        }
    }

    void NonInteractableBoard()
    {
        int x = Random.Range(0, 9);
        int y = Random.Range(0, 7);

        Vector2 randomCoords = new Vector2(x, y);

        if(_Model.virtualGrid.TryGetValue(randomCoords, out GridCell cell))
        {
            _poolManager.DeSpawnBlockView(cell.blockInCell.blockKind, cell.blockInCell.blockView);
            cell.ResetGridCell();
        }

        GameObject boosterObject = _poolManager.SpawnBlockView(ElementKind.BoosterRowColumn, cell.blockAnchorCoords);
        _View.FillGidCellWithBooster(cell.blockAnchorCoords, boosterObject, new BoosterBomb());
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
        foreach (var item in _Model.virtualGrid)
        {
            if (SimulateCheckInteractionWith(item.Value))
                return true;
        }

        return false;
    }
    bool SimulateCheckInteractionWith(GridCell gridCell)
    {
        foreach (Vector2 coords in gridCell.blockAnchorCoords.GetCrossCoords())
        {
            if (_Model.virtualGrid.TryGetValue(coords, out GridCell objectiveCell) &&
                gridCell.blockInCell != null && objectiveCell.blockInCell != null &&
                gridCell.blockInCell.blockKind == objectiveCell.blockInCell.blockKind)
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}