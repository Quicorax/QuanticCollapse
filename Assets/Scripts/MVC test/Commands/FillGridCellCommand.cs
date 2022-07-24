using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGridCellCommand : IGridCommand
{
    private PoolManager _poolManager;

    private Vector2 _coordsToFill;
    private ElementKind _blockKind;
    public FillGridCellCommand(PoolManager poolManager, Vector2 coordsToFill)
    {
        _poolManager = poolManager;
        _coordsToFill = coordsToFill;
    }
    public void Do(VirtualGridModel Model)
    {
        _blockKind = (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length - 1);
        Model.virtualGrid[_coordsToFill].SetDynamicBlockOnCellV2(new DynamicBlockV2(_blockKind, _coordsToFill, _poolManager.SpawnBlockView(_blockKind, _coordsToFill)));

        //Debug.Log("Filled grid cell at: " + _coordsToFill + " with block kind: " + _blockKind);
    }
}