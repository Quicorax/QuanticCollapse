using UnityEngine;

public class FillGridCellWithBoosterCommand : IGridCommand
{
    private BaseBooster _baseBooster;
    private Vector2 _coordsToFill;
    private GameObject _boosterObject;

    public FillGridCellWithBoosterCommand(BaseBooster baseBooster, Vector2 coordsToFill, GameObject boosterObject)
    {
        _coordsToFill = coordsToFill;
        _baseBooster = baseBooster;
        _boosterObject = boosterObject;
    }
    public void Do(VirtualGridModel Model)
    {
        Model.virtualGrid[_coordsToFill].SetDynamicBlockOnCellV2(new DynamicBlockV2(ElementKind.Booster, _coordsToFill, _boosterObject, true, _baseBooster));
    }
}
