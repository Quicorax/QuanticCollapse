using UnityEngine;

public class BlockLaserCommand : IGridCommand
{
    private GridInteractionSubController _interactionsLogic;
    private Vector2Int _inputCoords;

    public BlockLaserCommand(GridInteractionSubController InteractionsLogic, Vector2Int coords)
    {
        _interactionsLogic = InteractionsLogic;
        _inputCoords = coords;
    }

    public void Do(GridModel Model)
    {
        if (Model.virtualGrid.TryGetValue(_inputCoords, out GridCellController gridCell))
        { 
            _interactionsLogic.InteractionAtGrid(false, gridCell);
        }
    }
}

