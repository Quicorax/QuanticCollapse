using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInteractionsController : MonoBehaviour
{
    private VirtualGridView _View;
    private VirtualGridModel Model;

    [SerializeField] private AddScoreEventBus _AddScoreEventBus;
    [SerializeField] private GenericEventBus _BlockDestructionEventBus;

    [SerializeField] private BoostersLogic _boostersLogic;
    [SerializeField] private UserInputManager _userInputManager;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private TurnManager _turnManager;

    private List<GridCellController> autoclickOpenList = new();

    private int boostersInGrid;
    public void InteractionAtGrid(bool isRegularInput, GridCellController gridCell, VirtualGridView View, VirtualGridModel Model)
    {
        if (this.Model == null)
            this.Model = Model;
        if (_View == null)
            _View = View;

        if (!gridCell.CheckHasBlock())
            return;

        if (isRegularInput)
            InteractionAtGridCell(gridCell);
        else
            LaserBlock(gridCell);
    }
    void LaserBlock(GridCellController gridCell)
    {
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

        autoclickOpenList.Add(gridCell);
        while (autoclickOpenList.Count > 0)
        {
            GridCellController tappedGridCell = autoclickOpenList[0];
            autoclickOpenList.RemoveAt(0);
            InteractionCore(tappedGridCell, autoInput);

            Model.matchList.Clear();
            Model.boosterGridCell = null;
            autoInput = true;
            yield return new WaitForSeconds(0.5f);
        }
  

        autoclickOpenList.Clear();
        _userInputManager.BlockInputByGridInteraction(false);
    }
    void InteractionCore(GridCellController gridCell, bool autoInput)
    {
        if (CheckInteractionWith(gridCell))
        {
            AddScoreOnInteractionSucceed();

            if(!autoInput)
                _turnManager.InteractionUsed();

            DestroyBlocksOnActionSucceed();

            CheckForBoosterSpawnOnInteractionSucceed(gridCell.AnchorCoords);

            Invoke(nameof(RegenerateGrid), .25f);
        }
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
        {
            OpenClosedListMatchCellsGetter(gridCell);
        }
        else
        {
            CheckActionOnBoosterBased(gridCell);
            boosterMatchInteraction = true;
        }

        return Model.matchList.Count >= 2 || boosterMatchInteraction;
    }

    void CheckActionOnBoosterBased(GridCellController gridCell)
    {
        Model.boosterGridCell = gridCell;
        gridCell.CallBoosterInteraction(gridCell.GetBlockCoords(), Model);
    }

    void OpenClosedListMatchCellsGetter(GridCellController touchedGridCell)
    {
        //Close list is: _Model.matchList 
        List<GridCellController > openList = new();

        if (!touchedGridCell.CheckHasBlock())
            return;

        openList.Add(touchedGridCell);

        while (openList.Count > 0)
        {
            GridCellController selectedGridCell = openList[0];
            openList.RemoveAt(0);
            Model.matchList.Add(selectedGridCell);

            foreach (Vector2Int coords in selectedGridCell.GetBlockCoords().GetCrossCoords())
            {
                if (Model.virtualGrid.TryGetValue(coords, out GridCellController objectiveCell) && objectiveCell.CheckHasBlock() &&
                    touchedGridCell.GetBlockKind() == objectiveCell.GetBlockKind() && 
                    !openList.Contains(objectiveCell) && !Model.matchList.Contains(objectiveCell))
                {
                    openList.Add(objectiveCell);
                }
            }
        }
    }

    void AddScoreOnInteractionSucceed()
    {
        int elementCount = 0;
        ElementKind matchKind = Model.matchList[0].GetBlockKind();

        foreach (var CellController in Model.matchList)
        {
            if (!CellController.CheckIsBooster())
            {
                matchKind = CellController.GetBlockKind();
                elementCount++;
            }
        }

        _BlockDestructionEventBus.NotifyEvent();

        if(matchKind != ElementKind.BoosterRowColumn && 
            matchKind != ElementKind.BoosterBomb && 
            matchKind != ElementKind.BoosterKindBased)
        {
            _AddScoreEventBus.NotifyEvent(matchKind, elementCount);
        }
    }

    void DestroyBlocksOnActionSucceed()
    {
        if(Model.boosterGridCell != null)
        {
            boostersInGrid--;

            _poolManager.DeSpawnBlockView(Model.boosterGridCell.GetBlockKind(), Model.boosterGridCell.GetViewReference());
            Model.boosterGridCell.RemoveBlock();
        }

        foreach (var dynamicBlock in Model.matchList)
        {
            if (dynamicBlock.CheckIsBooster())
            {
                dynamicBlock.SetIsTriggered(true);
            }
            else
            {
                SingleBlockDestruction(dynamicBlock);
            }
        }
    }
    void SingleBlockDestruction(GridCellController dynamicBlock)
    {
        _poolManager.DeSpawnBlockView(dynamicBlock.GetBlockKind(), dynamicBlock.GetViewReference());
        dynamicBlock.RemoveBlock();
    }

    void CheckForBoosterSpawnOnInteractionSucceed(Vector2Int coords)
    {
        if (Model.boosterGridCell != null)
            return;

        if(_boostersLogic.CheckBaseBoosterSpawn(Model.matchList.Count, out BaseBooster booster))
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
        foreach (var element in Model.virtualGrid)
        {
            if (element.Value.CheckHasBlock())
            {
               int cellCollapseSteps = 0;

                for (int y = element.Key.y; y >= 0; y--)
                {
                    if (Model.virtualGrid.TryGetValue(new Vector2Int(element.Key.x, y), out GridCellController gridCell) && !gridCell.CheckHasBlock())
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
        foreach (var gridCell in Model.virtualGrid.Values)
        { 
            if(gridCell.CheckHasBlock() && gridCell.CheckCollapseSteps() > 0)
            {
                Vector2Int newCoords = gridCell.GetBlockCoords() + Vector2Int.down * gridCell.CheckCollapseSteps();

                GridCellController Controller = Model.virtualGrid[newCoords];
                Controller.SetDynamicBlockOnCell(gridCell.GetModel());
                Controller.SetCoords(newCoords);
                Controller.SetCollapseSteps(0);

                Controller.GetViewReference().transform.DOMoveY(newCoords.y, 0.4f).SetEase(Ease.OutBounce);

                gridCell.RemoveBlock();
            }
        }
    }

    void GenerateBlocksOnEmptyCells()
    {
        foreach (var item in Model.virtualGrid)
        {
            if (!item.Value.CheckHasBlock())
            {
                _View.FillGidCell(item.Key);
            }
        }

        if (!CheckImposibleBeard())
        {
            NonInteractableBoard();
        }
    }

    void CheckTriggeredBoostersToInteract()
    {
        foreach (var gridCell in Model.virtualGrid.Values)
        {
            if (gridCell.CheckHasBlock() && gridCell.CheckIsTriggered() && !autoclickOpenList.Contains(gridCell))
            {
                autoclickOpenList.Add(gridCell);
            }
        }
    }

    void NonInteractableBoard()
    {
        int x = Random.Range(0, 9);
        int y = Random.Range(0, 7);

        Vector2Int randomCoords = new Vector2Int(x, y);

        if(Model.virtualGrid.TryGetValue(randomCoords, out GridCellController cell))
        {
            _poolManager.DeSpawnBlockView(cell.GetBlockKind(), cell.GetViewReference());
            cell.RemoveBlock();
        }

        GameObject boosterObject = _poolManager.SpawnBlockView(ElementKind.BoosterRowColumn, cell.GetBlockCoords());
        _View.FillGidCellWithBooster(cell.GetBlockCoords(), boosterObject, new BoosterRowColumn());
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
        foreach (var item in Model.virtualGrid)
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
            if (Model.virtualGrid.TryGetValue(coords, out GridCellController objectiveCell) &&
                gridCell.CheckHasBlock() && objectiveCell.CheckHasBlock() &&
                gridCell.GetBlockKind() == objectiveCell.GetBlockKind())
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}