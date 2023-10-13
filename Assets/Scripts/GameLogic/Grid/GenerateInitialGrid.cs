using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuanticCollapse
{
    public class GenerateInitialGrid
    {
        private readonly PoolManager _poolManager;
        private readonly GridModel _model;

        private readonly GameConfigService _config;

        private readonly Dictionary<Vector2Int, int> _initialCellsDisposition = new();

        public GenerateInitialGrid(GridModel model, PoolManager poolManager)
        {
            _model = model;
            _poolManager = poolManager;
            _config = ServiceLocator.GetService<GameConfigService>();
        }

        public void Initialize(LevelModel levelModel)
        {
            var index = 0;

            for (var i = 0; i < levelModel.LevelHeight; i++)
            {
                for (var e = 0; e < levelModel.LevelWidth; e++)
                {
                    Vector2Int coords = new(e, i);
                    _initialCellsDisposition.Add(coords, levelModel.LevelDisposition[index]);
                    index++;
                    Do(new(coords));
                }
            }
        }

        private void Do(GridCellModel gridCell)
        {
            var _blockKind = CheckHandPlacementData(gridCell.AnchorCoords);
            gridCell.BlockModel = new(_blockKind, gridCell.AnchorCoords);

            _model.GridObjects.Add(gridCell.AnchorCoords,
                _poolManager.SpawnBlockView(_blockKind, gridCell.AnchorCoords));
            _model.GridData.Add(gridCell.AnchorCoords, gridCell);
        }

        private int CheckHandPlacementData(Vector2Int cellCoords)
        {
            if (_initialCellsDisposition.TryGetValue(cellCoords, out var cellKindIndex) && cellKindIndex != 9)
                return cellKindIndex;

            var rng = Random.Range(0, _config.GridBlocks.BaseBlocks.Count());
            return _config.GridBlocks.BaseBlocks[rng].Id;
        }
    }
}