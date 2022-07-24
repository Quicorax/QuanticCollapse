using System.Collections.Generic;
using UnityEngine;

public class InitialViewGridGeneration : MonoBehaviour
{
    private VirtualGridView _virtualGridView;
    private Vector2Int _gridDimensions = new Vector2Int(9, 7);

    private Vector2Int[] _cachedCoordsToCheck;
    private List<Vector2Int> _coordsToCheckList = new();

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
    }

    void GenerateGridCells()
    {
        for (int x = 0; x < _gridDimensions.x; x++)
        {
            for (int y = 0; y < _gridDimensions.y; y++)
            {
                Vector2Int gridCellCoords = new(x, y);
                _coordsToCheckList.Add(gridCellCoords);

                GridCell newGridCell = new(gridCellCoords);

                _virtualGridView.GenerateGidCell(gridCellCoords, newGridCell);
            }
        }
        _cachedCoordsToCheck = _coordsToCheckList.ToArray();
    }

   void FillGridCells()
   {
       foreach (Vector2Int cachedCoords in _cachedCoordsToCheck)
       {
       _virtualGridView.FillGidCell(cachedCoords);
       }
   }
}
