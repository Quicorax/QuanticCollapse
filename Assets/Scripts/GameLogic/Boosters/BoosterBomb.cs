using System.Collections.Generic;
using UnityEngine;

namespace QuanticCollapse
{
    public class BoosterBomb : BaseBooster
    {
        public int BoosterKindId { get; }

        public BoosterBomb(int id)
        {
            BoosterKindId = id;
        }

        public void OnInteraction(Vector2Int initialCoords, GridModel gridModel)
        {
            var coordsToCheck = new List<Vector2Int>();
            coordsToCheck.AddRange(initialCoords.GetSplashCoords());

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