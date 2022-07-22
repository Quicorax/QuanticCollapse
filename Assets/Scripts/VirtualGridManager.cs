using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
public enum ElementKind { Attack, Defense, Intel, Speed, Booster };

public class VirtualGridManager : MonoBehaviour
{
    public CellKindDeclarer cellKindDeclarer;
    public AggrupationManager aggrupationManager;
    public BoostersLogic boostersLogic;
    public PoolManager poolManager;

    public Dictionary<Vector2, GridCell> virtualGrid = new();

    [SerializeField]
    private BoosterActionEventBus _BoosterActionPositionBasedEventBus;
    [SerializeField]
    private BoosterActionEventBus _BoosterActionKindBasedEventBus;
    [SerializeField]
    private GenericEventBus _PlayerInteractionEventBus;
    [SerializeField]
    private GenericEventBus _ImpossibleGridEventBus;
    [SerializeField]
    private GenericEventBus _ExternalBoosterEventBus;
    [SerializeField]
    private TapOnCoordsEventBus _TapOnCoordsEventBus;
    [SerializeField]
    private AddScoreEventBus _AddScoreEventBus;

    void Awake()
    {
        _TapOnCoordsEventBus .Event += CheckElementOnGrid;
        _ImpossibleGridEventBus.Event += ResetGrid;

        _BoosterActionPositionBasedEventBus.Event += CellBoosterDestruction;
        _BoosterActionKindBasedEventBus.Event += CellSameKindDestruction;

    }
    void OnDestroy()
    {
        _TapOnCoordsEventBus.Event -= CheckElementOnGrid;
        _ImpossibleGridEventBus.Event -= ResetGrid;

        _BoosterActionPositionBasedEventBus.Event -= CellBoosterDestruction;
        _BoosterActionKindBasedEventBus.Event -= CellSameKindDestruction;
    }

    void Start()
    {
        GridCellSpawning(true);
    }

    void ResetGrid()
    {
        foreach (var item in virtualGrid.Values)
        {
            poolManager.DeSpawnBlockView(item.blockInCell.blockKind, item.blockInCell.debugBlockGraphic);

            item.ResetGridCell();
        }

        GridCellSpawning(false);
    }

    void GridCellSpawning(bool initial)
    {
        FillGridCells(initial);
        InitAggrupation();
    }


    void FillGridCells(bool initial)
    {
        foreach (var emptyGridCells in virtualGrid)
        {
            if (!emptyGridCells.Value.hasBlock)
            {
                GenerateCellBlock(emptyGridCells.Key, initial);
            }
        }
    }

    void GenerateCellBlock(Vector3 coords, bool initial)
    {
        ElementKind kind = cellKindDeclarer.GetCellKind(initial, coords);

        DynamicBlock newDynamicBlock = new(this, coords, kind);
        virtualGrid[coords].SetDynamicBlockOnCell(newDynamicBlock);

        newDynamicBlock.debugBlockGraphic = poolManager.SpawnBlockView(kind, coords);
    }

    void InitAggrupation()
    {
        foreach (var selfBlock in virtualGrid.Values)
        {
            if (selfBlock.hasBlock && !selfBlock.blockInCell.partOfAggrupation)
                selfBlock.blockInCell.CheckCrossNeightboursToAgrupate();
        }
    }


    #region Interaction Loop
    public void CheckElementOnGrid(Vector2 coords, bool isExternalBoosterInput)
    {
        if (virtualGrid.TryGetValue(coords, out GridCell gridCell) && gridCell.blockInCell != null)
        {
            if (isExternalBoosterInput)
            {
                DestroySingleBlock(gridCell.blockInCell);
                _ExternalBoosterEventBus.NotifyEvent();
                return;
            }

            if (gridCell.blockInCell.isBooster)
            {
                BoosterInteraction(gridCell.blockInCell);
                return;
            }
            if (gridCell.blockInCell.partOfAggrupation)
                AggrupationInteraction(gridCell.blockInCell);
        }
    }
    void BoosterInteraction(DynamicBlock dynamicBlock)
    {
        //Booster Actions
        dynamicBlock.selfBooster.OnInteraction(dynamicBlock.actualCoords);
        _PlayerInteractionEventBus.NotifyEvent();

        //Destroy Booster block
        DestroySingleBlock(dynamicBlock);
    }

    void DestroySingleBlock(DynamicBlock dynamicBlock)
    {
        if (dynamicBlock.isBooster)
        {
            Destroy(virtualGrid[dynamicBlock.actualCoords].blockInCell.debugBlockGraphic);
        }
        else
        { 
            poolManager.DeSpawnBlockView(virtualGrid[dynamicBlock.actualCoords].blockInCell.blockKind, virtualGrid[dynamicBlock.actualCoords].blockInCell.debugBlockGraphic);
        }
        
        UpperCellsPrepareCollapse(dynamicBlock.actualCoords);
        virtualGrid[dynamicBlock.actualCoords].blockInCell.mustGetDeleted = true;

        CollapseUpperGridElements();
        GridCellSpawning(false);
    }

    void AggrupationInteraction(DynamicBlock dynamicBlock)
    {
        int aggrupationIndex = dynamicBlock.aggrupationIndex;
        bool isBooster = false;

        if (aggrupationManager.GetAggrupationByItsIndex(aggrupationIndex, out Aggrupation aggrupationContainer))
        {
            _PlayerInteractionEventBus.NotifyEvent();
            _AddScoreEventBus.NotifyEvent(dynamicBlock.blockKind, aggrupationContainer.memberCoords.Count);

            if (boostersLogic.CheckBaseBoosterSpawn(aggrupationContainer.memberCoords.Count, out BaseBooster booster))
            {
                isBooster = true;
            }

            foreach (Vector2 coords in aggrupationContainer.memberCoords)
            {
                if (isBooster && coords == dynamicBlock.actualCoords)
                {
                    GameObject debugBlockGraphic = Instantiate(booster.boosterPrefab, coords, Quaternion.identity);
                    TransformBlockToBooster(coords, booster, debugBlockGraphic);
                    continue;
                }

                poolManager.DeSpawnBlockView(virtualGrid[coords].blockInCell.blockKind, virtualGrid[coords].blockInCell.debugBlockGraphic);

                UpperCellsPrepareCollapse(coords);
                virtualGrid[coords].blockInCell.mustGetDeleted = true;
            }
            aggrupationManager.TryDeleteAggrupationEntry(aggrupationIndex);
        }

        CollapseUpperGridElements();
        GridCellSpawning(false);
    }


    public void UpperCellsPrepareCollapse(Vector2 coords)
    {
        foreach (var cellPair in virtualGrid)
        {
            if (cellPair.Value.hasBlock && cellPair.Key.y > coords.y && cellPair.Key.x == coords.x)
            {
                cellPair.Value.blockInCell.mustCollapse = true;
                cellPair.Value.blockInCell.collapseSteps++;
            }
        }
    }
    void CollapseUpperGridElements()
    {
        foreach (GridCell cell in virtualGrid.Values)
        {
            if (cell.blockInCell != null)
            {
                if (cell.blockInCell.mustGetDeleted)
                {
                    CallResetCell(cell.blockInCell.actualCoords);
                    continue;
                }
                if (cell.blockInCell.mustCollapse)
                {
                    RepositionCell(cell.blockInCell);
                }
            }
        }
    }

    void RepositionCell(DynamicBlock dynamicBlock)
    {
        Vector2 oldCoords = dynamicBlock.actualCoords;
        Vector2 newCoords = dynamicBlock.actualCoords + Vector2.down * dynamicBlock.collapseSteps;

        aggrupationManager.RemoveElementFromAggrupation(dynamicBlock);
        
        RepositionedBlockDataUpdate(dynamicBlock, newCoords);
        virtualGrid[newCoords].SetDynamicBlockOnCell(dynamicBlock);

        dynamicBlock.debugBlockGraphic.transform.DOMoveY(newCoords.y, 0.3f);

        CallResetCell(oldCoords, true);
    }
    public void RepositionedBlockDataUpdate(DynamicBlock block, Vector2 newCoords)
    {
        block.actualCoords = newCoords;
        block.partOfAggrupation = false;
        block.aggrupationIndex = 0;
        block.mustCollapse = false;
        block.collapseSteps = 0;
    }

    public void TransformBlockToBooster(Vector2 coords, BaseBooster booster, GameObject debugBlockGraphic)
    {
        DynamicBlock blockToChange = virtualGrid[coords].blockInCell;

        poolManager.DeSpawnBlockView(blockToChange.blockKind, blockToChange.debugBlockGraphic);

        blockToChange.blockKind = ElementKind.Booster;
        blockToChange.isBooster = true;
        blockToChange.selfBooster = booster;

        blockToChange.debugBlockGraphic = debugBlockGraphic;
    }
    void CallResetCell(Vector2 coords, bool calledFromReposition = false)
    {
        if (!calledFromReposition)
        {
            aggrupationManager.RemoveElementFromAggrupation(virtualGrid[coords].blockInCell);
        }

        virtualGrid[coords].ResetGridCell();
    }


    #endregion

    #region Boosters Reaction
    void CellSameKindDestruction(Vector2[] coords)
    {
        ElementKind kind = cellKindDeclarer.RandomElementKind();
        
        foreach (Vector2 cellCoords in coords)
        {
            if (virtualGrid.TryGetValue(cellCoords, out GridCell cell) && cell.blockInCell != null && cell.blockInCell.blockKind == kind)
            {
                CellAction(cell);
            }
        }
    }


    void CellBoosterDestruction(Vector2[] coords)
    {
        foreach (Vector2 cellCoords in coords)
        {
            if (virtualGrid.TryGetValue(cellCoords, out GridCell cell) && cell.blockInCell != null && !cell.blockInCell.isBooster)
            {
                CellAction(cell);
            }
        }
    }

    void CellAction(GridCell cell)
    {
        _AddScoreEventBus.NotifyEvent(cell.blockInCell.blockKind, 1);

        poolManager.DeSpawnBlockView(cell.blockInCell.blockKind, cell.blockInCell.debugBlockGraphic);
        UpperCellsPrepareCollapse(cell.blockInCell.actualCoords);
        cell.blockInCell.mustGetDeleted = true;
    }
    #endregion
}
