using System.Collections.Generic;
using UnityEngine;

public class GridModel
{
    public Dictionary<Vector2Int, GridCellController> virtualGrid = new();

    bool setPlayerMaxHealth;
    private int playerHealth;
    public int playerMaxHealth;

    public int PlayerLife
    {
        get => playerHealth;
        set
        {
            if (!setPlayerMaxHealth)
            {
                playerMaxHealth = value;
                setPlayerMaxHealth = true;
            }

            playerHealth = value;
            Debug.Log("Player Life: " + value);
        }
    }


    bool setEnemyMaxHealth;
    private int enemyHealth;
    public int enemyMaxHealth;

    public int EnemyLife
    {
        get => enemyHealth;
        set
        {
            if (!setEnemyMaxHealth)
            {
                enemyMaxHealth = value;
                setEnemyMaxHealth = true;
            }
            enemyHealth = value;
            Debug.Log("Enemy Life: " + value);
        }
    }
}