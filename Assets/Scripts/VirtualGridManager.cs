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

    public DynamicBlock(VirtualGridManager virtualGridManager, Vector2 actualCoords, ElementKind blockKind)
    {
        this.virtualGridManager = virtualGridManager;
        this.actualCoords = actualCoords;
        this.blockKind = blockKind;
    }

    public void CheckCrossNeightboursToAgrupate(bool inheritedAggrupation = false, int inheritedIndex = 0)
    {
        int newAggrupationIndex = inheritedAggrupation ? inheritedIndex : virtualGridManager.GetAggrupationInex();

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

        bool aggrupationRegistered = virtualGridManager.aggrupationList.TryGetValue(index, out var existingAggrupationList);
        if (aggrupationRegistered && !existingAggrupationList.Contains(actualCoords))
        {
            existingAggrupationList.Add(actualCoords);
        }
        else if (!aggrupationRegistered)
        {
            List<Vector2> newAggrupationList = new();
            newAggrupationList.Add(actualCoords);

            virtualGridManager.aggrupationList.Add(index, newAggrupationList);
        }

        if (virtualGridManager.spawnDebugGraphics)
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
public class VirtualGridManager : MonoBehaviour
{
    public LevelManager starshipManager;
    public LevelData levelData;
    public BlockPoolManager poolManager;

    public Dictionary<Vector2, GridCell> virtualGrid = new();
    public Dictionary<int, List<Vector2>> aggrupationList = new();

    public bool spawnDebugGraphics;

    public Color[] colorData;

    int aggrupationIndexAmount;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        GenerateGridCells();
        FillGridCells(true);
        InitAggrupation();
    }

    void GenerateGridCells()
    {
        for (int x = 0; x < levelData.gridDimensions.x; x++)
        {
            for (int y = 0; y < levelData.gridDimensions.y; y++)
            {
                Vector2 gridCellCoords = new(x, y);
                GridCell newGridCell = new();

                virtualGrid.Add(gridCellCoords, newGridCell);
            }
        }
    }
    void FillGridCells(bool initialGeneration)
    {
        foreach (var newCelBlock in virtualGrid)
        {
            if (!newCelBlock.Value.hasBlock)
            {
                ElementKind kind = SetKindToNewDynamicCell(newCelBlock.Key, initialGeneration);
                DynamicBlock newDynamicBlock = new(this, newCelBlock.Key, kind);

                virtualGrid[newCelBlock.Key].SetDynamicBlockOnCell(newDynamicBlock);

                if (spawnDebugGraphics)
                    newDynamicBlock.debugBlockGraphic = poolManager.SpawnFromPool(kind, newCelBlock.Key);
            }
        }
    }
    void InitAggrupation()
    {
        foreach (var selfBlock in virtualGrid.Values)
        {
            if (selfBlock.hasBlock && !selfBlock.blockInCell.partOfAggrupation)
                selfBlock.blockInCell.CheckCrossNeightboursToAgrupate();
        }
    }

    ElementKind SetKindToNewDynamicCell(Vector2 cellCoords, bool initialGeneration)
    {
        if (initialGeneration && levelData.gridInitialLayout != null && CheckHandPlacementData(cellCoords, out ElementKind kind))
            return kind;
        return RandomElementKind();
    }
    bool CheckHandPlacementData(Vector2 cellCoords, out ElementKind kind)
    {
        Color pixelColor = levelData.gridInitialLayout.GetPixel((int)cellCoords.x, (int)cellCoords.y);
        for (int i = 0; i < colorData.Length; i++)
        {
            if (pixelColor == colorData[i])
            {
                kind = (ElementKind)i;
                return true;
            }
        }
        kind = RandomElementKind();
        return false;
    }
    ElementKind RandomElementKind()
    {
        //return ExtensionMethods.GetRandomElementKind<ElementKind>();
        return (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length);
    }
    public int GetAggrupationInex() { return aggrupationIndexAmount++; }

    #region Interaction Loop
    public void CheckElementOnGrid(Vector2 coords)
    {
        if (virtualGrid.TryGetValue(coords, out GridCell gridCell) && gridCell.blockInCell != null && gridCell.blockInCell.partOfAggrupation)
            AggrupationInteraction(gridCell.blockInCell);
    }
    void AggrupationInteraction(DynamicBlock dynamicBlock)
    {
        starshipManager.InteractionUsed(dynamicBlock.blockKind, aggrupationList[dynamicBlock.aggrupationIndex].Count);

        int aggrupationIndex = dynamicBlock.aggrupationIndex;
        foreach (Vector2 coords in aggrupationList[aggrupationIndex])
        {
            if (spawnDebugGraphics)
                poolManager.DeSpawnObject(virtualGrid[coords].blockInCell.blockKind, virtualGrid[coords].blockInCell.debugBlockGraphic);

            UpperCellsPrepareRepositioning(coords);
            virtualGrid[coords].blockInCell.mustGetDeleted = true;
        }

        aggrupationList.Remove(aggrupationIndex);

        RepositionUpperGridElements();

        FillGridCells(false);
        InitAggrupation();
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

        dynamicBlock.RepositionedBlockDataUpdate(newCoords);
        virtualGrid[newCoords].SetDynamicBlockOnCell(dynamicBlock);

        if (spawnDebugGraphics)
            dynamicBlock.debugBlockGraphic.transform.DOMoveY(newCoords.y, 0.3f);

        CallResetCell(oldCoords);
    }
    void CallResetCell(Vector2 coords)
    {
        virtualGrid[coords].ResetGridCell();
    }
    #endregion
}
