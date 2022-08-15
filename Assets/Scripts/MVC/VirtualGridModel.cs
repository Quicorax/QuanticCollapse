using System.Collections.Generic;
using UnityEngine;

public class VirtualGridModel
{
    public List<GridCell> matchList = new();
    public GridCell boosterGridCell;

    public Dictionary<Vector2, GridCell> virtualGrid = new();

    bool setPlayerMaxLife;
    public int PlayerLife 
    { 
        get => playerLife;  
        set 
        {
            if (!setPlayerMaxLife)
            {
                playerMaxLife = value;
                setPlayerMaxLife = true;
            }

            playerLife = value; Debug.Log("Player Life: " + value);
        } 
    }

    private int playerLife;

    public int playerMaxLife;
    public int EnemyLife { get => enemyLife;  set { enemyLife = value; Debug.Log("Enemy Life: " + value); } }
    private int enemyLife;

}