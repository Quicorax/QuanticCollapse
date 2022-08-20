using System.Collections.Generic;
using UnityEngine;

public class VirtualGridModel
{
    public List<GridCell> matchList = new();
    public GridCell boosterGridCell;

    public Dictionary<Vector2, GridCell> virtualGrid = new();

    bool setPlayerMaxLife;
    private int playerLife;
    public int playerMaxLife;

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

            playerLife = value;
            Debug.Log("Player Life: " + value);
        }
    }


    bool setEnemyMaxLife;
    private int enemyLife;
    public int enemyMaxLife;

    public int EnemyLife
    {
        get => enemyLife;
        set
        {
            if (!setEnemyMaxLife)
            {
                enemyMaxLife = value;
                setEnemyMaxLife = true;
            }
            enemyLife = value;
            Debug.Log("Enemy Life: " + value);
        }
    }
}