using UnityEngine;

public class BlockLaserCommand : IGridCommand
{
    private VirtualGridView _View;

    private GridInteractionsController _interactionsLogic;
    private Vector2 _inputCoords;

    public BlockLaserCommand(VirtualGridView view, GridInteractionsController InteractionsLogic, Vector2 inputCoords)
    {
        _View = view;
        _interactionsLogic = InteractionsLogic;
        _inputCoords = inputCoords;
    }

    public void Do(VirtualGridModel Model)
    {
        if (!Model.virtualGrid.TryGetValue(_inputCoords, out GridCell gridCell) && gridCell.hasBlock || gridCell.blockInCell.isBooster)
            return;

        _interactionsLogic.LaserBlock(gridCell, _View, Model);
    }
}

