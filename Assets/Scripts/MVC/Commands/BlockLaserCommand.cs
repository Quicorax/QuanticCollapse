using UnityEngine;

public class BlockLaserCommand : IGridCommand
{

    private GridInteractionsController _interactionsLogic;
    private Vector2Int _inputCoords;

    public BlockLaserCommand(GridInteractionsController InteractionsLogic, Vector2Int coords)
    {
        _interactionsLogic = InteractionsLogic;
        _inputCoords = coords;
    }

    public void Do(VirtualGridModel Model)
    {
        if (!Model.virtualGrid.TryGetValue(_inputCoords, out GridCellController gridCell))
        { 
            _interactionsLogic.InteractionAtGrid(false, gridCell, Model);
        }
    }
}

