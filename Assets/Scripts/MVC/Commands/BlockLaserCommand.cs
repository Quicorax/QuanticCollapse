using UnityEngine;

public class BlockLaserCommand : IGridCommand
{
    private VirtualGridView _View;

    private GridInteractionsController _interactionsLogic;
    private Vector2 _inputCoords;

    public BlockLaserCommand(VirtualGridView view, GridInteractionsController InteractionsLogic, Vector2 coords)
    {
        _interactionsLogic = InteractionsLogic;
        _inputCoords = coords;
        _View = view;
    }

    public void Do(VirtualGridModel Model)
    {
        if (!Model.virtualGrid.TryGetValue(_inputCoords, out GridCell gridCell))
            return;

        _interactionsLogic.LaserBlock(gridCell, _View, Model);
    }
}

