using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGridCellCommand : IGridCommand
{
    private Vector2 _coordsToFill;
    private ElementKind _blockKind;
    public FillGridCellCommand(Vector2 coordsToFill) => _coordsToFill = coordsToFill;

    public void Do(VirtualGridModel Model)
    {
        _blockKind = (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length - 1);
        Model.virtualGrid[_coordsToFill].blockInCellV2 = new DynamicBlockV2(_blockKind);

        Debug.Log("Filled grid cell at: " + _coordsToFill + " with block kind: " + _blockKind);
    }
}
