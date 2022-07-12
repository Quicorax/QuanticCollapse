using UnityEngine;

[RequireComponent(typeof(VirtualGridManager))]
public class BaseGridGeneration : MonoBehaviour
{
    VirtualGridManager virtualGridManager;

    void Awake()
    {
        virtualGridManager = GetComponent<VirtualGridManager>();
    }
    void Start()
    {
        GenerateGridCells();
    }
    void GenerateGridCells()
    {
        for (int x = 0; x < virtualGridManager.gridData.gridDimensions.x; x++)
        {
            for (int y = 0; y < virtualGridManager.gridData.gridDimensions.y; y++)
            {
                Vector2 gridCellCoords = new(x, y);
                GridCell newGridCell = new();

                virtualGridManager.virtualGrid.Add(gridCellCoords, newGridCell);
            }
        }
    }
}
