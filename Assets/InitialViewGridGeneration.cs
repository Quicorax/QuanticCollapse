using UnityEngine;

public class InitialViewGridGeneration : MonoBehaviour
{
    private VirtualGridView _virtualGridView;
    private Vector2 _gridDimensions = new Vector2(9, 7);

    private Vector2[] cachedCoordsToCheck = new Vector2[63];

    private void Awake()
    {
        _virtualGridView = GetComponent<VirtualGridView>();
    }
    private void Start()
    {
        InitialGeneration();
    }

    void InitialGeneration()
    {
        GenerateGridCells();
        FillGridCells();
        AggrupateGridCells();
    }

    void GenerateGridCells()
    {
        for (int x = 0; x < _gridDimensions.x; x++)
        {
            for (int y = 0; y < _gridDimensions.y; y++)
            {
                Vector2 gridCellCoords = new(x, y);
                cachedCoordsToCheck[x] = gridCellCoords;

                GridCell newGridCell = new();

                _virtualGridView.GenerateGidCell(gridCellCoords, newGridCell);
            }
        }
    }

    void FillGridCells()
    {
        foreach (Vector2 cachedCoords in cachedCoordsToCheck)
            _virtualGridView.FillGidCell(cachedCoords);
    }

    void AggrupateGridCells()
    {
        _virtualGridView.AggrupateBlocks();
    }
}
