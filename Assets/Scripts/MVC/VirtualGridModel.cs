using System.Collections.Generic;
using UnityEngine;

public class VirtualGridModel
{
    public List<GridCell> matchList = new();
    public GridCell boosterGridCell;

    public Dictionary<Vector2, GridCell> virtualGrid = new();

    public int PlayerLife { get => playerLife;  set { playerLife = value; Debug.Log("Player Life: " + value); } }
    private int playerLife;
    public int EnemyLife { get => enemyLife;  set { enemyLife = value; Debug.Log("Enemy Life: " + value); } }
    private int enemyLife;

}