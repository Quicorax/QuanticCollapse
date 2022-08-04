using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInteractionsController : MonoBehaviour
{
    private VirtualGridView _View;
    public VirtualGridModel Model;

    [SerializeField] private AddScoreEventBus _AddScoreEventBus;

    [SerializeField] private BoostersLogic _boostersLogic;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private TurnManager _turnManager;

    //public Vector2[] particleContainerCoords;
    //public GameObject[] particles;

    private bool generationComplete;
    private int boostersInGrid;

    public void LaserBlock(GridCell gridCell, VirtualGridView View = null, VirtualGridModel Model = null)
    {
        if (this.Model == null)
            this.Model = Model;
        if (_View == null)
            _View = View;

        SingleBlockDestruction(gridCell);

        Invoke(nameof(RegenerateGrid), 0.25f);
    }
    public void InteractionAtGridCell(GridCell gridCell, VirtualGridView View = null, VirtualGridModel Model = null)
    {
        if (this.Model == null)
            this.Model = Model;
        if (_View == null)
            _View = View;

        if (gridCell.blockInCell == null)
            return;

        InteractionCore(gridCell);

        this.Model.matchList.Clear();
        this.Model.boosterGridCell = null;
    }

    void InteractionCore(GridCell gridCell)
    {
        if (CheckInteractionWith(gridCell))
        {
            generationComplete = false;

            if(!gridCell.blockInCell.isTriggered)
                _turnManager.InteractionUsed();

            AddScoreOnInteractionSucceed();

            DestroyBlocksOnActionSucceed();

            CheckForBoosterSpawnOnInteractionSucceed(gridCell.blockAnchorCoords);

            Invoke(nameof(RegenerateGrid), 0.25f);
        }
    }
    void RegenerateGrid()
    {
        CheckCollapseBoard();

        GenerateBlocksOnEmptyCells();

        StartCoroutine(CheckTriggeredBoostersToInteract());
    }
    bool CheckInteractionWith(GridCell gridCell)
    {
        bool boosterInteraction = false;

        if (!gridCell.blockInCell.isBooster)
        {
            OpenClosedListMatchCellsGetter(gridCell);
        }
        else
        {
            CheckActionOnBoosterBased(gridCell);
            boosterInteraction = true;
        }

        return Model.matchList.Count >= 2 || boosterInteraction;
    }

    void CheckActionOnBoosterBased(GridCell gridCell)
    {
        Model.boosterGridCell = gridCell;
        gridCell.blockInCell.booster.OnInteraction(gridCell.blockAnchorCoords, Model);
    }

    void OpenClosedListMatchCellsGetter(GridCell touchedGridCell)
    {
        //Close list is: _Model.matchList 
        List<GridCell> openList = new();

        if (!touchedGridCell.hasBlock)
            return;

        openList.Add(touchedGridCell);

        while (openList.Count > 0)
        {
            GridCell selectedGridCell = openList[0];
            openList.RemoveAt(0);
            Model.matchList.Add(selectedGridCell);

            foreach (Vector2 coords in selectedGridCell.blockAnchorCoords.GetCrossCoords())
            {
                if (Model.virtualGrid.TryGetValue(coords, out GridCell objectiveCell) && objectiveCell.hasBlock &&
                    touchedGridCell.blockInCell.blockKind == objectiveCell.blockInCell.blockKind && 
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
        ElementKind matchKind = Model.matchList[0].blockInCell.blockKind;

        foreach (var item in Model.matchList)
        {
            if (!item.blockInCell.isBooster)
            {
                matchKind = item.blockInCell.blockKind;
                elementCount++;
            }
        }

        _AddScoreEventBus.NotifyEvent(matchKind, elementCount);
    }

    void DestroyBlocksOnActionSucceed()
    {
        if(Model.boosterGridCell != null)
        {
            boostersInGrid--;

            _poolManager.DeSpawnBlockView(Model.boosterGridCell.blockInCell.blockKind, Model.boosterGridCell.blockInCell.blockView);
            Model.boosterGridCell.ResetGridCell();
        }

        foreach (var dynamicBlock in Model.matchList)
        {
            if (dynamicBlock.blockInCell.isBooster)
            {
                dynamicBlock.blockInCell.isTriggered = true;
            }
            else
            {
                //SetViewParticles(dynamicBlock.blockAnchorCoords, dynamicBlock.blockInCell.blockKind);

                SingleBlockDestruction(dynamicBlock);
            }
        }
    }
    void SingleBlockDestruction(GridCell dynamicBlock)
    {
        _poolManager.DeSpawnBlockView(dynamicBlock.blockInCell.blockKind, dynamicBlock.blockInCell.blockView);
        dynamicBlock.ResetGridCell();
    }

    //void SetViewParticles(Vector2 originalCoords, ElementKind kind)
    //{
    //    Transform particle = Instantiate(particles[(int)kind], originalCoords, Quaternion.identity).transform;
    //
    //    particle.DOMove(particleContainerCoords[(int)kind], 0.3f).OnComplete(()=> { Destroy(particle.gameObject); });
    //}

    void CheckForBoosterSpawnOnInteractionSucceed(Vector2 coords)
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
            if (element.Value.hasBlock)
            {
               int cellCollapseSteps = 0;

                for (int y = (int)element.Key.y; y >= 0; y--)
                {
                    if (Model.virtualGrid.TryGetValue(new Vector2(element.Key.x, y), out GridCell gridCell) && !gridCell.hasBlock)
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
        foreach (var gridCell in Model.virtualGrid.Values)
        { 
            if(gridCell.hasBlock && gridCell.blockInCell.collapseSteps > 0)
            {
                Vector2 newCoords = gridCell.blockInCell.blockCoords + Vector2.down * gridCell.blockInCell.collapseSteps;
                DynamicBlock dynamicBlock = gridCell.blockInCell;

                dynamicBlock.blockCoords = newCoords;
                dynamicBlock.collapseSteps = 0;

                dynamicBlock.blockView.transform.DOMoveY(newCoords.y, 0.4f).SetEase(Ease.OutBounce);

                Model.virtualGrid[newCoords].SetDynamicBlockOnCell(dynamicBlock);
                gridCell.ResetGridCell();
            }
        }
    }

    void GenerateBlocksOnEmptyCells()
    {
        foreach (var item in Model.virtualGrid)
        {
            if (!item.Value.hasBlock)
            {
                _View.FillGidCell(item.Key);
            }
        }

        generationComplete = true;

        if (!CheckImposibleBeard())
        {
            NonInteractableBoard();
        }
    }

    IEnumerator CheckTriggeredBoostersToInteract()
    {
        foreach (var item in Model.virtualGrid)
        {
            if (item.Value.blockInCell != null && item.Value.blockInCell.isTriggered)
            {
                yield return new WaitUntil(()=> generationComplete);
                yield return new WaitForSeconds(0.5f);

                _View.ListenInput(item.Key, false);
            }
        }
    }

    void NonInteractableBoard()
    {
        int x = Random.Range(0, 9);
        int y = Random.Range(0, 7);

        Vector2 randomCoords = new Vector2(x, y);

        if(Model.virtualGrid.TryGetValue(randomCoords, out GridCell cell))
        {
            _poolManager.DeSpawnBlockView(cell.blockInCell.blockKind, cell.blockInCell.blockView);
            cell.ResetGridCell();
        }

        GameObject boosterObject = _poolManager.SpawnBlockView(ElementKind.BoosterRowColumn, cell.blockAnchorCoords);
        _View.FillGidCellWithBooster(cell.blockAnchorCoords, boosterObject, new BoosterRowColumn());
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
    bool SimulateCheckInteractionWith(GridCell gridCell)
    {
        foreach (Vector2 coords in gridCell.blockAnchorCoords.GetCrossCoords())
        {
            if (Model.virtualGrid.TryGetValue(coords, out GridCell objectiveCell) &&
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