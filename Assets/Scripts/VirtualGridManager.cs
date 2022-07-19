using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
public enum ElementKind { Attack, Defense, Intel, Speed, Booster };

[System.Serializable]
public struct Aggrupation
{
    public int index;
    public List<Vector2> memberCoords;
}
public class VirtualGridManager : MonoBehaviour
{
    public CellKindDeclarer cellKindDeclarer;
    public BlockPoolManager poolManager;

    public BoostersLogic boostersLogic;

    public Dictionary<Vector2, GridCell> virtualGrid = new();
    public List<Aggrupation> aggrupationList = new();
    Aggrupation emptyAggrupation = new();

    public bool spawnGraphics;

    public int aggrupationIndexAmount;

    void Awake()
    {
        EventManager.Instance.OnTapp += CheckElementOnGrid;

    }
    void OnDestroy()
    {
        EventManager.Instance.OnTapp -= CheckElementOnGrid;
    }

    void Start()
    {
        Init();
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

    void Init()
    {
        GridCellSpawning(true);
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

        if (aggrupationList.Count == 0)
        {
            ResetGrid();
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

        if (GetAggrupationByItsIndex(aggrupationIndex, out Aggrupation aggrupationContainer))
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
                    virtualGrid[coords].blockInCell.SetBlockToBooster(booster, debugBlockGraphic);
                    continue;
                }

                if (spawnGraphics)
                    poolManager.DeSpawnObject(virtualGrid[coords].blockInCell.blockKind, virtualGrid[coords].blockInCell.debugBlockGraphic);

                UpperCellsPrepareCollapse(coords);
                virtualGrid[coords].blockInCell.mustGetDeleted = true;
            }
            DeleteAggrupationEntry(aggrupationIndex);
        }

        CollapseUpperGridElements();
        GridCellSpawning(false);
    }

    public void DeleteAggrupationEntry(int index)
    {
        if (GetAggrupationByItsIndex(index, out Aggrupation aggrupation))
            aggrupationList.Remove(aggrupation);
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

        RemoveElementFromAggrupation(dynamicBlock, oldCoords);

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
            RemoveElementFromAggrupation(virtualGrid[coords].blockInCell, coords);
            //DeleteAggrupationEntry(virtualGrid[coords].blockInCell.aggrupationIndex);
        }

        virtualGrid[coords].ResetGridCell();
    }

    void RemoveElementFromAggrupation(DynamicBlock block, Vector2 elementCoords)
    {
        if (GetAggrupationByItsIndex(block.aggrupationIndex, out Aggrupation aggrupation))
        {
            aggrupation.memberCoords.Remove(elementCoords);

            if (aggrupation.memberCoords.Count == 0)
                aggrupationList.Remove(aggrupation);
        }
    }

    public bool GetAggrupationByItsIndex(int index, out Aggrupation aggrupation)
    {
        foreach (var item in aggrupationList)
        {
            if (item.index == index)
            {
                aggrupation = item;
                return true;
            }
        }
        aggrupation = emptyAggrupation;
        return false;
    }
    #endregion
}
