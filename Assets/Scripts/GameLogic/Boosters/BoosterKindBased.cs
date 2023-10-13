using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuanticCollapse
{
    public class BoosterKindBased : BaseBooster
    {
        private readonly GameConfigService _config;
        public int BoosterKindId { get; }

        public BoosterKindBased(int id)
        {
            _config = ServiceLocator.GetService<GameConfigService>();
            BoosterKindId = id;
        }

        public void OnInteraction(Vector2Int initialCoords, GridModel gridModel)
        {
            List<Vector2Int> coordsToCheck = new();
            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 7; y++)
                {
                    coordsToCheck.Add(new Vector2Int(x, y));
                }
            }

            var rng = Random.Range(0, _config.GridBlocks.BaseBlocks.Count());
            var kindId = _config.GridBlocks.BaseBlocks[rng].Id;

            foreach (var coords in coordsToCheck)
            {
                if (gridModel.GridData.TryGetValue(coords, out var cell) &&
                    cell.BlockModel != null && cell.BlockModel.Id == kindId)
                {
                    gridModel.MatchClosedList.Add(cell);
                }
            }
        }
    }
}