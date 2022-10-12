using UnityEngine;

public class BlockLaserCommand
{
    private GridController _interactionsLogic;
    private Vector2Int _inputCoords;

    public BlockLaserCommand(GridController InteractionsLogic, Vector2Int coords)
    {
        _interactionsLogic = InteractionsLogic;
        _inputCoords = coords;
    }

    public void Do(GridModel Model)
    {
        if (Model.GridData.TryGetValue(_inputCoords, out GridCellController gridCell))
        { 
            _interactionsLogic.InteractionAtGrid(false, gridCell);
        }
    }
}

