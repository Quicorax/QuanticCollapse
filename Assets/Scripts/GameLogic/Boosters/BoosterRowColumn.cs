using System.Collections.Generic;
using UnityEngine;

namespace QuanticCollapse
{
    public class BoosterRowColumn : BaseBooster
    {
        public int BoosterKindId { get; }

        public BoosterRowColumn(int id)
        {
            BoosterKindId = id;
        }

        public void OnInteraction(Vector2Int initialCoords, GridModel gridModel)
        {
            var vertical = Random.Range(0, 100) > 50;
            List<Vector2Int> coordsToCheck = new();

            if (vertical)
            {
                for (var index = 0; index < 7; index++)
                {
                    coordsToCheck.Add(new Vector2Int(initialCoords.x, index));
                }
            }
            else
            {
                for (var index = 0; index < 9; index++)
                {
                    coordsToCheck.Add(new Vector2Int(index, initialCoords.y));
                }
            }

            coordsToCheck.Remove(initialCoords);

            foreach (var coords in coordsToCheck)
            {
                if (gridModel.GridData.TryGetValue(coords, out var cell) && cell.BlockModel != null)
                {
                    gridModel.MatchClosedList.Add(cell);
                }
            }
        }
    }
}