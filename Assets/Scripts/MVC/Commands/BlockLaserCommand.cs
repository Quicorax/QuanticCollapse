using UnityEngine;

public class BlockLaserCommand : IGridCommand
{
    private VirtualGridView _View;

    private GridInteractionsController _interactionsLogic;
    private Vector2Int _inputCoords;

    public BlockLaserCommand(VirtualGridView view, GridInteractionsController InteractionsLogic, Vector2Int coords)
    {
        _interactionsLogic = InteractionsLogic;
        _inputCoords = coords;
        _View = view;
    }

    public void Do(VirtualGridModel Model)
    {
        if (!Model.virtualGrid.TryGetValue(_inputCoords, out GridCellController gridCell))
        { 
            _interactionsLogic.InteractionAtGrid(false, gridCell, _View, Model);
        }
    }
}

