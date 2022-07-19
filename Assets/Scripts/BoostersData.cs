using System.Collections.Generic;
using UnityEngine;

public class BaseBooster
{
    public VirtualGridManager virtualGridManager;

    public int interactiosToSpawn;
    public string boosterName;
    public string boosterDescription;

    public GameObject boosterPrefab;

    public virtual void OnInteraction(Vector2 initialCoords)
    {
    }
}
[System.Serializable]
public class BoosterA : BaseBooster
{
    public override void OnInteraction(Vector2 initialCoords)
    {
        foreach (GridCell cell in virtualGridManager.virtualGrid.Values)
        {
            if (cell.blockInCell != null && !cell.blockInCell.isBooster && cell.blockInCell.actualCoords != initialCoords)
            {
                if (cell.blockInCell.actualCoords.x == initialCoords.x)
                {
                    EventManager.Instance.AddScoreBlock(cell.blockInCell.blockKind, 1);

                    virtualGridManager.poolManager.DeSpawnObject(cell.blockInCell.blockKind, cell.blockInCell.debugBlockGraphic);
                    cell.blockInCell.mustGetDeleted = true;
                }
            }
        }
    }
}
[System.Serializable]
public class BoosterB : BaseBooster
{
    public override void OnInteraction(Vector2 initialCoords)
    {
        foreach (GridCell cell in virtualGridManager.virtualGrid.Values)
        {
            if (cell.blockInCell != null && !cell.blockInCell.isBooster && cell.blockInCell.actualCoords != initialCoords)
            {
                if (cell.blockInCell.actualCoords.y == initialCoords.y)
                {
                    EventManager.Instance.AddScoreBlock(cell.blockInCell.blockKind, 1);

                    virtualGridManager.poolManager.DeSpawnObject(cell.blockInCell.blockKind, cell.blockInCell.debugBlockGraphic);
                    virtualGridManager.UpperCellsPrepareCollapse(cell.blockInCell.actualCoords);
                    cell.blockInCell.mustGetDeleted = true;
                }
            }
        }
    }
}
[System.Serializable]
public class BoosterC : BaseBooster
{
    public override void OnInteraction(Vector2 initialCoords)
    {
        ElementKind kind = virtualGridManager.cellKindDeclarer.RandomElementKind();

        foreach (GridCell cell in virtualGridManager.virtualGrid.Values)
        {
            if (cell.blockInCell != null && cell.blockInCell.blockKind == kind)
            {
                EventManager.Instance.AddScoreBlock(cell.blockInCell.blockKind, 1);

                virtualGridManager.poolManager.DeSpawnObject(cell.blockInCell.blockKind, cell.blockInCell.debugBlockGraphic);
                virtualGridManager.UpperCellsPrepareCollapse(cell.blockInCell.actualCoords);
                cell.blockInCell.mustGetDeleted = true;
            }
        }
    }
}


[CreateAssetMenu(fileName = "BoostersData", menuName = "ScriptableObjects/BoostersData", order = 3)]
public class BoostersData : ScriptableObject
{
    public BoosterA boosterA;
    public BoosterB boosterB;
    public BoosterC boosterC;

    public bool GetBooster(int index, out BaseBooster booster)
    {
        if(index >= boosterC.interactiosToSpawn)
        {
            booster = boosterC;
            return true;
        }
        else if (index >= boosterB.interactiosToSpawn)
        {
            booster = boosterB;
            return true;
        }
        else if (index >= boosterA.interactiosToSpawn)
        {
            booster = boosterA;
            return true;
        }

        booster = null;
        return false;
    }
}
