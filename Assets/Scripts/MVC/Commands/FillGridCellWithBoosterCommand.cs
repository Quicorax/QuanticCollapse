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
    public void Do(VirtualGridModel Model)
    {
        Model.virtualGrid[_coordsToFill].SetDynamicBlockOnCell(new BlockModel(_baseBooster.boosterKind, _coordsToFill, _boosterObject, _baseBooster));
    }
}
