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

        bool aggrupationRegistered = virtualGridManager.aggrupationDictionary.TryGetValue(index, out var existingAggrupationDictionary);
        if (aggrupationRegistered && !existingAggrupationDictionary.Contains(actualCoords))
        {
            existingAggrupationDictionary.Add(actualCoords);
        }
        else if (!aggrupationRegistered)
        {
            List<Vector2> newAggrupationList = new();
            newAggrupationList.Add(actualCoords);

            virtualGridManager.aggrupationDictionary.Add(index, newAggrupationList);
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
    public Dictionary<int, List<Vector2>> aggrupationDictionary = new();

    public bool spawnDebugGraphics;

    public Color[] colorData;

    public int aggrupationIndexAmount;

    void Start()
    {
        Init();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ResetGrid();
        }
    }
    void ResetGrid()
    {
        foreach (var item in virtualGrid.Values)
        {
            if (spawnDebugGraphics)
                poolManager.DeSpawnObject(item.blockInCell.blockKind, item.blockInCell.debugBlockGraphic);

            item.ResetGridCell();
        }

        GridFillment(false);
    }

    void Init()
    {
        GenerateGridCells();
        GridFillment(true);
    }

    void GridFillment(bool initial)
    {
        FillGridCells(initial);
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

        if (aggrupationDictionary.Count == 0)
        {
            ResetGrid();
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

    #region Interaction Loop
    public void CheckElementOnGrid(Vector2 coords)
    {
        if (virtualGrid.TryGetValue(coords, out GridCell gridCell) && gridCell.blockInCell != null && gridCell.blockInCell.partOfAggrupation)
            AggrupationInteraction(gridCell.blockInCell);
    }
    void AggrupationInteraction(DynamicBlock dynamicBlock)
    {
        starshipManager.InteractionUsed(dynamicBlock.blockKind, aggrupationDictionary[dynamicBlock.aggrupationIndex].Count);

        int aggrupationIndex = dynamicBlock.aggrupationIndex;
        foreach (Vector2 coords in aggrupationDictionary[aggrupationIndex])
        {
            if (spawnDebugGraphics)
                poolManager.DeSpawnObject(virtualGrid[coords].blockInCell.blockKind, virtualGrid[coords].blockInCell.debugBlockGraphic);

            UpperCellsPrepareRepositioning(coords);
            virtualGrid[coords].blockInCell.mustGetDeleted = true;
        }

        DeleteAggrupationEntry(aggrupationIndex);

        RepositionUpperGridElements();
        GridFillment(false);
    }
    public void DeleteAggrupationEntry(int index) { aggrupationDictionary.Remove(index); }

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

        aggrupationDictionary.Remove(dynamicBlock.aggrupationIndex);

        dynamicBlock.RepositionedBlockDataUpdate(newCoords);
        virtualGrid[newCoords].SetDynamicBlockOnCell(dynamicBlock);

        if (spawnDebugGraphics)
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
