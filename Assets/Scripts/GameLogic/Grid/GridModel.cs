using System.Collections.Generic;
using UnityEngine;

namespace QuanticCollapse
{
    public class GridModel
    {
        public List<GridCellModel> MatchClosedList = new();
        public List<GridCellModel> MatchOpenList = new();

        public Dictionary<Vector2Int, GridCellModel> GridData = new();
        public Dictionary<Vector2Int, GameObject> GridObjects = new();

        public bool IsPlayerMaxHealthSet;
        public int PlayerHealth;
        public int PlayerMaxHealth;


        public bool IsEnemyMaxHealthSet;
        public int EnemyHealth;
        public int EnemyMaxHealth;
    }
}