using UnityEngine;

[CreateAssetMenu]
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
