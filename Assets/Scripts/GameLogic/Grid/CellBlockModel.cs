using UnityEngine;

namespace QuanticCollapse
{
    public class CellBlockModel
    {
        public int Id;
        public Vector2Int Coords;
        public BaseBooster Booster;
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