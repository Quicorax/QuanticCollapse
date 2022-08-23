using UnityEngine;

public class GenerateGridCellCommand : IGridCommand
{
    private Vector2Int _coords;
    private GridCellController _gridCell;
    public GenerateGridCellCommand(Vector2Int coords, GridCellController gridCell)
    {
        _coords = coords;
        _gridCell = gridCell;
    }
    public void Do(VirtualGridModel Model)
    {
        Model.virtualGrid.Add(_coords, _gridCell);
    }
}
