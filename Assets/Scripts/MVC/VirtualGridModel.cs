using System.Collections.Generic;
using UnityEngine;

public class VirtualGridModel
{
    public int gridWidht = 9;
    public int gridHeight = 7;

    public List<GridCell> matchList = new();
    public GridCell boosterGridCell;

    public Dictionary<Vector2, GridCell> virtualGrid = new();

    public int playerLife;
    public int enemyLife;

}