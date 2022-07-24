using UnityEngine;

public class UserInteractionCommand : IGridCommand
{
    private VirtualGridView _View;

    private GridInteractionsController _interactionsLogic;
    private Vector2 _inputCoords;


    public UserInteractionCommand(VirtualGridView View, GridInteractionsController InteractionsLogic, Vector2 coords)
    {
        _interactionsLogic = InteractionsLogic;
        _inputCoords = coords;
        _View = View;
    } 

    public void Do(VirtualGridModel Model)
    {
        //Debug.Log("Input at: " + _inputCoords);

        if (!Model.virtualGrid.TryGetValue(_inputCoords, out GridCell gridCell))
            return;

        _interactionsLogic.InteractionAtGridCell(gridCell, _View, Model);
    }
}


