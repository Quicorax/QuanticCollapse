using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterReaction : MonoBehaviour
{
    [SerializeField]
    private BoosterActionEventBus _BoosterActionPositionBasedEventBus;
    [SerializeField]
    private BoosterActionEventBus _BoosterActionKindBasedEventBus;
    [SerializeField]
    private AddScoreEventBus _AddScoreEventBus;

    public PoolManager poolManager;
    public CellKindDeclarer cellKindDeclarer;
    public VirtualGridManager virtualGridManager;

    void Awake()
    {

        _BoosterActionPositionBasedEventBus.Event += CellBoosterDestruction;
        _BoosterActionKindBasedEventBus.Event += CellSameKindDestruction;

    }
    void OnDestroy()
    {
        _BoosterActionPositionBasedEventBus.Event -= CellBoosterDestruction;
        _BoosterActionKindBasedEventBus.Event -= CellSameKindDestruction;
    }
    void CellSameKindDestruction(Vector2[] coords)
    {
        ElementKind kind = cellKindDeclarer.RandomElementKind();

        foreach (Vector2 cellCoords in coords)
        {
            if (virtualGridManager.virtualGrid.TryGetValue(cellCoords, out GridCell cell) && cell.blockInCell != null && cell.blockInCell.blockKind == kind)
            {
                CellAction(cell);
            }
        }
    }

    void CellBoosterDestruction(Vector2[] coords)
    {
        foreach (Vector2 cellCoords in coords)
        {
            if (virtualGridManager.virtualGrid.TryGetValue(cellCoords, out GridCell cell) && cell.blockInCell != null && !cell.blockInCell.isBooster)
            {
                CellAction(cell);
            }
        }
    }

    void CellAction(GridCell cell)
    {
        _AddScoreEventBus.NotifyEvent(cell.blockInCell.blockKind, 1);

        poolManager.DeSpawnBlockView(cell.blockInCell.blockKind, cell.blockInCell.debugBlockGraphic);
        virtualGridManager.UpperCellsPrepareCollapse(cell.blockInCell.actualCoords);
        cell.blockInCell.mustGetDeleted = true;
    }
}
