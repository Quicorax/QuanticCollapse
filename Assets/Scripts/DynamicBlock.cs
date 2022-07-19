using System.Collections.Generic;
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
    public bool mustCollapse;
    public int collapseSteps;

    public DynamicBlock(VirtualGridManager virtualGridManager, Vector2 actualCoords, ElementKind blockKind)
    {
        this.virtualGridManager = virtualGridManager;
        this.actualCoords = actualCoords;
        this.blockKind = blockKind;
    }

    public void CheckCrossNeightboursToAgrupate(bool inheritedAggrupation = false, int inheritedIndex = 0)
    {
        int newAggrupationIndex = inheritedAggrupation ? inheritedIndex : virtualGridManager.aggrupationManager.aggrupationIndexAmount++;

        if (inheritedAggrupation)
            virtualGridManager.aggrupationManager.TryDeleteAggrupationEntry(aggrupationIndex);

        foreach (var crossCoord in actualCoords.GetCrossCoords())
        {
            if (virtualGridManager.virtualGrid.TryGetValue(crossCoord, out GridCell neightbourCell) && neightbourCell.hasBlock && blockKind == neightbourCell.blockInCell.blockKind)
            {
                partOfAggrupation = true;
                aggrupationIndex = newAggrupationIndex;
                virtualGridManager.aggrupationManager.SetBlockCellOnAggrupation(newAggrupationIndex, neightbourCell.blockInCell.actualCoords);

                if (neightbourCell.blockInCell.aggrupationIndex != aggrupationIndex)
                    neightbourCell.blockInCell.CheckCrossNeightboursToAgrupate(true, newAggrupationIndex);
            }
        }
    }
   
    public void RepositionedBlockDataUpdate(Vector2 newCoords)
    {
        actualCoords = newCoords;

        partOfAggrupation = false;

        aggrupationIndex = 0;

        mustCollapse = false;
        collapseSteps = 0;
    }
    public void TransformBlockToBooster(BaseBooster booster, GameObject debugBlockGraphic)
    {
        virtualGridManager.poolManager.DeSpawnObject(blockKind, this.debugBlockGraphic);

        blockKind = ElementKind.Booster;
        isBooster = true;
        selfBooster = booster;
        selfBooster.virtualGridManager = virtualGridManager;

        this.debugBlockGraphic = debugBlockGraphic;
    }
}
