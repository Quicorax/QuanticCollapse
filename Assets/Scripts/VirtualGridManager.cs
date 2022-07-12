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

    public bool mustGetDeleted;
    public bool mustReposition;
    public int verticalRepositioningSteps;

    public DynamicBlock(VirtualGridManager virtualGridManager, Vector2 actualCoords, ElementKind blockKind /*, bool isBooster = false*/)
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
            if (virtualGridManager.virtualGrid.TryGetValue(crossCoord, out GridCell neightbourCell) &&
                neightbourCell.hasBlock &&
                blockKind == neightbourCell.blockInCell.blockKind)
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
        List<Vector2> existingAggrupationDictionary = null;

        foreach (var item in virtualGridManager.aggrupationList)
        {
            if (item.index == index)
            {
                aggrupationRegistered = true;
                existingAggrupationDictionary = item.memeberCoords;
            }
        }

        if (aggrupationRegistered && !existingAggrupationDictionary.Contains(actualCoords))
        {
            existingAggrupationDictionary.Add(actualCoords);
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
}
public class GridCell
{
    public bool hasBlock;
    public DynamicBlock blockInCell;
    public GameObject debugGirdCellGraphic;

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
public enum ElementKind { Red, Green, Blue, Yellow };

[System.Serializable]
public struct Aggrupation
{
    public int index;
    public List<Vector2> memeberCoords;
}
public class VirtualGridManager : MonoBehaviour
{
    public LevelGridData gridData;
    public CellKindDeclarer gridDataDecrypter;

    public LevelManager starshipManager;

    public BlockPoolManager poolManager;

    public Dictionary<Vector2, GridCell> virtualGrid = new();
    //public Dictionary<int, List<Vector2>> aggrupationDictionary = new();
    public List<Aggrupation> aggrupationList = new();

    public bool spawnGraphics;

    public int aggrupationIndexAmount;

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
        foreach (var newCelBlock in virtualGrid)
        {
            if (!newCelBlock.Value.hasBlock)
            {
                GenerateCellBlock(newCelBlock.Key, initial);
            }
        }
    }

    void GenerateCellBlock(Vector3 coords, bool initial)
    {
        ElementKind kind = initial ? gridDataDecrypter.CheckHandPlacementData(coords) : gridDataDecrypter.RandomElementKind();
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
        if (virtualGrid.TryGetValue(coords, out GridCell gridCell) && gridCell.blockInCell != null && gridCell.blockInCell.partOfAggrupation)
            AggrupationInteraction(gridCell.blockInCell);
    }
    void AggrupationInteraction(DynamicBlock dynamicBlock)
    {
        //starshipManager.InteractionUsed(dynamicBlock.blockKind, aggrupationDictionary[dynamicBlock.aggrupationIndex].Count);

        int aggrupationIndex = dynamicBlock.aggrupationIndex;

        foreach (var item in aggrupationList)
        {
            if (item.index == aggrupationIndex)
            {
                foreach (Vector2 coords in item.memeberCoords)
                {
                    if (spawnGraphics)
                        poolManager.DeSpawnObject(virtualGrid[coords].blockInCell.blockKind, virtualGrid[coords].blockInCell.debugBlockGraphic);

                    UpperCellsPrepareRepositioning(coords);

                    virtualGrid[coords].blockInCell.mustGetDeleted = true;
                }
            }
        }

        DeleteAggrupationEntry(aggrupationIndex);

        RepositionUpperGridElements();
        GridCellSpawning(false);
    }

    public void DeleteAggrupationEntry(int index)
    {
        foreach (var item in aggrupationList)
        {
            if (item.index == index)
            {
                aggrupationList.Remove(item);
                break;
            }
        }
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
    void RepositionUpperGridElements()
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

        DeleteAggrupationEntry(dynamicBlock.aggrupationIndex);

        dynamicBlock.RepositionedBlockDataUpdate(newCoords);
        virtualGrid[newCoords].SetDynamicBlockOnCell(dynamicBlock);

        if (spawnGraphics)
            dynamicBlock.debugBlockGraphic.transform.DOMoveY(newCoords.y, 0.3f);

        CallResetCell(oldCoords);
    }
    void CallResetCell(Vector2 coords)
    {
        DeleteAggrupationEntry(virtualGrid[coords].blockInCell.aggrupationIndex);
        virtualGrid[coords].ResetGridCell();
    }

    #endregion
}
