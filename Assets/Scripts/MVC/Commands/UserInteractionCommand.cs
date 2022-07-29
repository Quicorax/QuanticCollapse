using UnityEngine;

public class UserInteractionCommand : IGridCommand
{
    private VirtualGridView _View;

    private GridInteractionsController _interactionsLogic;
    private Vector2 _inputCoords;
    private bool _boostedInput;


    public UserInteractionCommand(VirtualGridView View, GridInteractionsController InteractionsLogic, Vector2 coords, bool boostedInput)
    {
        _interactionsLogic = InteractionsLogic;
        _inputCoords = coords;
        _View = View;
        _boostedInput = boostedInput;
    } 

    public void Do(VirtualGridModel Model)
    {
        if (!Model.virtualGrid.TryGetValue(_inputCoords, out GridCell gridCell))
            return;

        _interactionsLogic.InteractionAtGridCell(gridCell, _View, Model, _boostedInput);
    }
}


