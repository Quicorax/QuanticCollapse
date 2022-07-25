using UnityEngine;

public class GenerateGridCellCommand : IGridCommand
{
    private Vector2 _coords;
    private GridCell _gridCell;
    public GenerateGridCellCommand(Vector2 coords, GridCell gridCell)
    {
        _coords = coords;
        _gridCell = gridCell;
    }
    public void Do(VirtualGridModel Model)
    {
        Model.virtualGrid.Add(_coords, _gridCell);
    }
}
