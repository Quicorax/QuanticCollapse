using UnityEngine;

public class UserInteractionCommand : IGridCommand
{
    private GridInteractionsController _interactionsLogic;
    private Vector2Int _inputCoords;

    public UserInteractionCommand(GridInteractionsController InteractionsLogic, Vector2Int coords)
    {
        _interactionsLogic = InteractionsLogic;
        _inputCoords = coords;
    } 

    public void Do(VirtualGridModel Model)
    {
        if (Model.virtualGrid.TryGetValue(_inputCoords, out GridCellController gridCell))
        { 
            _interactionsLogic.InteractionAtGrid(true, gridCell, Model);
        }
    }
}