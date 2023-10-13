using UnityEngine;

namespace QuanticCollapse
{
    public class CellBlockModel
    {
        public readonly int Id;
        public readonly BaseBooster Booster;
        public Vector2Int Coords;
        public int CollapseSteps;
        public bool IsTriggered;

        public CellBlockModel(int id, Vector2Int coords, BaseBooster booster = null)
        {
            Id = id;
            Coords = coords;
            Booster = booster;
        }
    }
}