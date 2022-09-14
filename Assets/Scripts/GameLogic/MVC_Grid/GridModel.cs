using System.Collections.Generic;
using UnityEngine;

public class GridModel
{
    public Dictionary<Vector2Int, GridCellController> VirtualGrid = new();

    public bool IsPlayerMaxHealthSet;
    public int PlayerHealth;
    public int PlayerMaxHealth;


    public bool IsEnemyMaxHealthSet;
    public int EnemyHealth;
    public int EnemyMaxHealth;
}