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
            existingAggrupationList = aggrupationContainer.memberCoords;
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
            newAggrupation.memberCoords = newAggrupationList;

            virtualGridManager.aggrupationList.Add(newAggrupation);
        }

        //if (virtualGridManager.spawnGraphics)
        //    debugBlockGraphic.transform.GetChild(1).GetComponent<TMP_Text>().text = index.ToString();
    }
    public void RepositionedBlockDataUpdate(Vector2 newCoords)
    {
        actualCoords = newCoords;

        partOfAggrupation = false;

        aggrupationIndex = 0;

        mustCollapse = false;
        collapseSteps = 0;
    }
    public void SetBlockToBooster(BaseBooster booster, GameObject debugBlockGraphic)
    {
        virtualGridManager.poolManager.DeSpawnObject(blockKind, this.debugBlockGraphic);

        blockKind = ElementKind.Booster;
        isBooster = true;
        selfBooster = booster;
        selfBooster.virtualGridManager = virtualGridManager;

        this.debugBlockGraphic = debugBlockGraphic;
    }
}
