using UnityEngine;

[CreateAssetMenu]
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
