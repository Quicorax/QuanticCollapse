using UnityEngine;

public class UserInteractionCommand : IGridCommand
{
    private VirtualGridView _View;

    private GridInteractionsController _interactionsLogic;
    private Vector2Int _inputCoords;

    public UserInteractionCommand(VirtualGridView View, GridInteractionsController InteractionsLogic, Vector2Int coords)
    {
        _interactionsLogic = InteractionsLogic;
        _inputCoords = coords;
        _View = View;
    } 

    public void Do(VirtualGridModel Model)
    {
        if (Model.virtualGrid.TryGetValue(_inputCoords, out GridCellController gridCell))
        { 
            _interactionsLogic.InteractionAtGrid(true, gridCell, _View, Model);
        }
    }
}