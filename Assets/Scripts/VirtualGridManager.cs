using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DynamicBlock
{
    public VirtualGridManager virtualGridManager;

    public Vector2 actualCoords;
    public ElementKind blockKind;

    public GameObject debugBlockGraphic;

    public bool partOfAggrupation;
    public int aggrupationIndex;

    public bool isBooster;
    public BaseBooster selfBooster;

    public bool mustGetDeleted;
    public bool mustReposition;
    public int verticalRepositioningSteps;

    public DynamicBlock(VirtualGridManager virtualGridManager, Vector2 actualCoords, ElementKind blockKind)
    {
        this.virtualGridManager = virtualGridManager;
        this.actualCoords = actualCoords;
        this.blockKind = blockKind;
    }

    public void CheckCrossNeightboursToAgrupate(bool inheritedAggrupation = false, int inheritedIndex = 0)
    {
        int newAggrupationIndex = inheritedAggrupation ? inheritedIndex : virtualGridManager.aggrupationIndexAmount++;

        if (inheritedAggrupation)
            virtualGridManager.DeleteAggrupationEntry(aggrupationIndex);

        foreach (var crossCoord in actualCoords.GetCrossCoords())
        {
            if (virtualGridManager.virtualGrid.TryGetValue(crossCoord, out GridCell neightbourCell) && neightbourCell.hasBlock && blockKind == neightbourCell.blockInCell.blockKind)
            {
                SetBlockCellOnAggrupation(newAggrupationIndex);

                if (neightbourCell.blockInCell.aggrupationIndex != aggrupationIndex)
                    neightbourCell.blockInCell.CheckCrossNeightboursToAgrupate(true, newAggrupationIndex);
            }
        }
    }
    public void SetBlockCellOnAggrupation(int index)
    {
        partOfAggrupation = true;
        aggrupationIndex = index;

        bool aggrupationRegistered = false;
        List<Vector2> existingAggrupationList = null;

        if (virtualGridManager.GetAggrupationByItsIndex(aggrupationIndex, out Aggrupation aggrupationContainer))
        {
            aggrupationRegistered = true;
            existingAggrupationList = aggrupationContainer.memeberCoords;
        }

        if (aggrupationRegistered && !existingAggrupationList.Contains(actualCoords))
        {
            existingAggrupationList.Add(actualCoords);
        }
        else if (!aggrupationRegistered)
        {
            List<Vector2> newAggrupationList = new();
            newAggrupationList.Add(actualCoords);

            Aggrupation newAggrupation = new Aggrupation();
            newAggrupation.index = index;
            newAggrupation.memeberCoords = newAggrupationList;

            virtualGridManager.aggrupationList.Add(newAggrupation);
        }

        if (virtualGridManager.spawnGraphics)
            debugBlockGraphic.transform.GetChild(1).GetComponent<TMP_Text>().text = index.ToString();
    }
    public void RepositionedBlockDataUpdate(Vector2 newCoords)
    {
        actualCoords = newCoords;

        partOfAggrupation = false;

        aggrupationIndex = 0;

        mustReposition = false;
        verticalRepositioningSteps = 0;
    }
    public void SetBlockToBooster(BaseBooster booster, GameObject debugBlockGraphic)
    {
        virtualGridManager.poolManager.DeSpawnObject(blockKind, this.debugBlockGraphic);

        blockKind = ElementKind.Booster;
        isBooster = true;
        selfBooster = booster;

        this.debugBlockGraphic = debugBlockGraphic;
    }

}
public class GridCell
{
    public bool hasBlock;
    public DynamicBlock blockInCell;

    public void SetDynamicBlockOnCell(DynamicBlock dynamicBlock)
    {
        hasBlock = true;
        blockInCell = dynamicBlock;
    }
    public void ResetGridCell()
    {
        hasBlock = false;
        blockInCell = null;
    }
}
public enum ElementKind { Attack, Defense, Intel, Speed, Booster };

[System.Serializable]
public struct Aggrupation
{
    public int index;
    public List<Vector2> memeberCoords;
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
            newDynamicBlock.debugBlockGraphic = poolManager.SpawnFromPool(kind, coords);
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
    public void CheckElementOnGrid(Vector2 coords)
    {
        if (virtualGrid.TryGetValue(coords, out GridCell gridCell) && gridCell.blockInCell != null)
        {
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
        dynamicBlock.selfBooster.OnInteraction(out int actionIndex);
        BoostersEffect(dynamicBlock.actualCoords, actionIndex);
        EventManager.Instance.Interaction();

        //Destroy Booster block
        Destroy(virtualGrid[dynamicBlock.actualCoords].blockInCell.debugBlockGraphic);
        UpperCellsPrepareRepositioning(dynamicBlock.actualCoords);
        virtualGrid[dynamicBlock.actualCoords].blockInCell.mustGetDeleted = true;

        CollapseUpperGridElements();
        GridCellSpawning(false);
    }

    void BoostersEffect(Vector2 initialCoords, int actionIndex)
    {
        switch (actionIndex)
        {
            case 1:
                BoosterAEffect(initialCoords);
                break;
            case 2:
                BoosterBEffect(initialCoords);
                break;
            case 3:
                BoosterDEffect();
                break;
        }
    }
    void BoosterAEffect(Vector2 initialCoords)
    {
        foreach (GridCell cell in virtualGrid.Values)
        {
            if (cell.blockInCell != null && !cell.blockInCell.isBooster && cell.blockInCell.actualCoords != initialCoords)
            {
                if (cell.blockInCell.actualCoords.y == initialCoords.y)
                {
                    EventManager.Instance.AddScoreBlock(cell.blockInCell.blockKind, 1);

                    poolManager.DeSpawnObject(cell.blockInCell.blockKind, cell.blockInCell.debugBlockGraphic);
                    UpperCellsPrepareRepositioning(cell.blockInCell.actualCoords);
                    cell.blockInCell.mustGetDeleted = true;
                }
            }
        }
    }
    void BoosterBEffect(Vector2 initialCoords)
    {
        Vector2[] coordsToCheck = initialCoords.GetSplashCoords();

        foreach (Vector2 bombCoords in coordsToCheck)
        {
            foreach (GridCell cell in virtualGrid.Values)
            {
                if (cell.blockInCell != null && !cell.blockInCell.isBooster && cell.blockInCell.actualCoords != initialCoords)
                {
                    if (cell.blockInCell.actualCoords == bombCoords)
                    {
                        EventManager.Instance.AddScoreBlock(cell.blockInCell.blockKind, 1);

                        poolManager.DeSpawnObject(cell.blockInCell.blockKind, cell.blockInCell.debugBlockGraphic);
                        UpperCellsPrepareRepositioning(cell.blockInCell.actualCoords);
                        cell.blockInCell.mustGetDeleted = true;
                    }
                }
            }
        }

    }
    void BoosterDEffect()
    {
        ElementKind kind = cellKindDeclarer.RandomElementKind();

        foreach (GridCell cell in virtualGrid.Values)
        {
            if (cell.blockInCell != null && cell.blockInCell.blockKind == kind)
            {
                EventManager.Instance.AddScoreBlock(cell.blockInCell.blockKind, 1);

                poolManager.DeSpawnObject(cell.blockInCell.blockKind, cell.blockInCell.debugBlockGraphic);
                UpperCellsPrepareRepositioning(cell.blockInCell.actualCoords);
                cell.blockInCell.mustGetDeleted = true;
            }
        }

    }
    void AggrupationInteraction(DynamicBlock dynamicBlock)
    {
        int aggrupationIndex = dynamicBlock.aggrupationIndex;
        bool isBooster = false;

        if (GetAggrupationByItsIndex(aggrupationIndex, out Aggrupation aggrupationContainer))
        {
            EventManager.Instance.Interaction();
            EventManager.Instance.AddScoreBlock(dynamicBlock.blockKind, aggrupationContainer.memeberCoords.Count);

            if (boostersLogic.CheckBaseBoosterSpawn(aggrupationContainer.memeberCoords.Count, out BaseBooster booster))
            {
                isBooster = true;
            }

            foreach (Vector2 coords in aggrupationContainer.memeberCoords)
            {
                if (isBooster && coords == dynamicBlock.actualCoords)
                {
                    GameObject debugBlockGraphic = Instantiate(booster.boosterPrefab, coords, Quaternion.identity);
                    virtualGrid[coords].blockInCell.SetBlockToBooster(booster, debugBlockGraphic);
                    continue;
                }

                if (spawnGraphics)
                    poolManager.DeSpawnObject(virtualGrid[coords].blockInCell.blockKind, virtualGrid[coords].blockInCell.debugBlockGraphic);

                UpperCellsPrepareRepositioning(coords);
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

    void UpperCellsPrepareRepositioning(Vector2 coords)
    {
        foreach (var cellPair in virtualGrid)
        {
            if (cellPair.Value.hasBlock && cellPair.Key.y > coords.y && cellPair.Key.x == coords.x)
            {
                cellPair.Value.blockInCell.mustReposition = true;
                cellPair.Value.blockInCell.verticalRepositioningSteps++;
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
                if (cell.blockInCell.mustReposition)
                {
                    RepositionCell(cell.blockInCell);
                }
            }
        }
    }

    void RepositionCell(DynamicBlock dynamicBlock)
    {
        Vector2 oldCoords = dynamicBlock.actualCoords;
        Vector2 newCoords = dynamicBlock.actualCoords + Vector2.down * dynamicBlock.verticalRepositioningSteps;

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
            DeleteAggrupationEntry(virtualGrid[coords].blockInCell.aggrupationIndex);
        }

        virtualGrid[coords].ResetGridCell();
    }

    void RemoveElementFromAggrupation(DynamicBlock block, Vector2 elementCoords)
    {
        if (GetAggrupationByItsIndex(block.aggrupationIndex, out Aggrupation aggrupation))
        {
            aggrupation.memeberCoords.Remove(elementCoords);

            if (aggrupation.memeberCoords.Count == 0)
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
