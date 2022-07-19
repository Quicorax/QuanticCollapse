using UnityEngine;

[CreateAssetMenu]
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
