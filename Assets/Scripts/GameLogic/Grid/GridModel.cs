using System.Collections.Generic;
using UnityEngine;

namespace QuanticCollapse
{
    public class GridModel
    {
        public readonly List<GridCellModel> MatchClosedList = new();
        public readonly List<GridCellModel> MatchOpenList = new();

        public readonly Dictionary<Vector2Int, GridCellModel> GridData = new();
        public readonly Dictionary<Vector2Int, GameObject> GridObjects = new();

        public bool IsPlayerMaxHealthSet;
        public int PlayerHealth;
        public int PlayerMaxHealth;


        public bool IsEnemyMaxHealthSet;
        public int EnemyHealth;
        public int EnemyMaxHealth;
    }
}