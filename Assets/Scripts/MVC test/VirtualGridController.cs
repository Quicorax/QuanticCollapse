using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualGridController 
{
    public VirtualGridModel Model = new VirtualGridModel();

    public bool TryGetGridCellByCoords(Vector2 coords, out GridCell gridCell)
    {
        return Model.virtualGrid.TryGetValue(coords, out gridCell);
    }
}
