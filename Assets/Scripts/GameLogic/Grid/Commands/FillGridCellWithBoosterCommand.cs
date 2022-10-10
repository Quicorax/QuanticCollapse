using UnityEngine;

public class FillGridCellWithBoosterCommand : IGridCommand
{
    private BaseBooster _baseBooster;
    private Vector2Int _coordsToFill;
    private GameObject _boosterObject;

    public FillGridCellWithBoosterCommand(BaseBooster baseBooster, Vector2Int coordsToFill, GameObject boosterObject)
    {
        _coordsToFill = coordsToFill;
        _baseBooster = baseBooster;
        _boosterObject = boosterObject;
    }
    public void Do(GridModel Model)
    {
        Model.VirtualGrid[_coordsToFill].SetDynamicBlockOnCell(new CellBlockModel(_baseBooster.BoosterKindId, _coordsToFill, _boosterObject, _baseBooster));
    }
}
