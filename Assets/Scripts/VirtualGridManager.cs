using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
public enum ElementKind { Attack, Defense, Intel, Speed, Booster };

public class VirtualGridManager : MonoBehaviour
{
    public CellKindDeclarer cellKindDeclarer;
    public AggrupationManager aggrupationManager;
    public BoostersLogic boostersLogic;
    public BlockPoolManager poolManager;

    public Dictionary<Vector2, GridCell> virtualGrid = new();

    public bool spawnGraphics;


    void Awake()
    {
        EventManager.Instance.OnTapp += CheckElementOnGrid;
        EventManager.Instance.OnImposibleGrid += ResetGrid;

    }
    void OnDestroy()
    {
        EventManager.Instance.OnTapp -= CheckElementOnGrid;
        EventManager.Instance.OnImposibleGrid -= ResetGrid;
    }

    void Start()
    {
        GridCellSpawning(true);
    }

    void ResetGrid()
    {
        foreach (var item in virtualGrid.Values)
        {
            if (spawnGraphics)
                poolManager.DeSpawnObject(item.blockInCell.blockKind, item.blockInCell.debugBlockGraphic);

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

        if (spawnGraphics)
            newDynamicBlock.debugBlockGraphic = poolManager.SpawnObject(kind, coords);
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
                EventManager.Instance.ExternalBoosterUsed();
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
        EventManager.Instance.Interaction();

        //Destroy Booster block
        DestroySingleBlock(dynamicBlock);
    }

    void DestroySingleBlock(DynamicBlock dynamicBlock)
    {
        Destroy(virtualGrid[dynamicBlock.actualCoords].blockInCell.debugBlockGraphic);
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
            EventManager.Instance.Interaction();
            EventManager.Instance.AddScoreBlock(dynamicBlock.blockKind, aggrupationContainer.memberCoords.Count);

            if (boostersLogic.CheckBaseBoosterSpawn(aggrupationContainer.memberCoords.Count, out BaseBooster booster))
            {
                isBooster = true;
            }

            foreach (Vector2 coords in aggrupationContainer.memberCoords)
            {
                if (isBooster && coords == dynamicBlock.actualCoords)
                {
                    GameObject debugBlockGraphic = Instantiate(booster.boosterPrefab, coords, Quaternion.identity);
                    virtualGrid[coords].blockInCell.TransformBlockToBooster(booster, debugBlockGraphic);
                    continue;
                }

                if (spawnGraphics)
                    poolManager.DeSpawnObject(virtualGrid[coords].blockInCell.blockKind, virtualGrid[coords].blockInCell.debugBlockGraphic);

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
        
        dynamicBlock.RepositionedBlockDataUpdate(newCoords);
        virtualGrid[newCoords].SetDynamicBlockOnCell(dynamicBlock);

        if (spawnGraphics)
            dynamicBlock.debugBlockGraphic.transform.DOMoveY(newCoords.y, 0.3f);

        CallResetCell(oldCoords, true);
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
}
