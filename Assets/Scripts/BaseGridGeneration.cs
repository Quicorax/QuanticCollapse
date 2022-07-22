using UnityEngine;

[RequireComponent(typeof(VirtualGridManager))]
public class BaseGridGeneration : MonoBehaviour
{
    Vector2 gridDimensions = new Vector2(9,7);
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
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                Vector2 gridCellCoords = new(x, y);
                GridCell newGridCell = new();

                virtualGridManager.virtualGrid.Add(gridCellCoords, newGridCell);
            }
        }
    }
}