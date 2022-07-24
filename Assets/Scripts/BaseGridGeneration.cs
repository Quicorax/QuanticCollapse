using UnityEngine;

public class BaseGridGeneration : MonoBehaviour
{
    private Vector2 _gridDimensions = new Vector2(9,7);
    private VirtualGridManager _virtualGridManager;


    void Awake()
    {
        _virtualGridManager = GetComponent<VirtualGridManager>();
    }
    void Start()
    {
        GenerateGridCells();
    }

    void GenerateGridCells()
    {
        for (int x = 0; x < _gridDimensions.x; x++)
        {
            for (int y = 0; y < _gridDimensions.y; y++)
            {
                Vector2 gridCellCoords = new(x, y);
                GridCell newGridCell = new(gridCellCoords);

                _virtualGridManager.virtualGrid.Add(gridCellCoords, newGridCell);
            }
        }
    }
}
