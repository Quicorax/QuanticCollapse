using System.Collections.Generic;
using UnityEngine;

public class VirtualGridModel
{
    public int[] turnScore = new int[4];

    public Dictionary<Vector2, GridCell> virtualGrid = new();
}
