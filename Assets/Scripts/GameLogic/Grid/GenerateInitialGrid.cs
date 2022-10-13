using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuanticCollapse
{
    public class GenerateInitialGrid
    {
        private PoolManager _poolManager;
        private GridModel _model;

        private GameConfigService _config;

        private Dictionary<Vector2Int, int> _initialCellsDisposition = new();
        public GenerateInitialGrid(GridModel model, PoolManager poolManager)
        {
            _model = model;
            _poolManager = poolManager;
            _config = ServiceLocator.GetService<GameConfigService>();
        }

        public void Initialize(LevelModel levelModel)
        {
            int index = 0;

            for (int i = 0; i < levelModel.LevelHeight; i++)
            {
                for (int e = 0; e < levelModel.LevelWidth; e++)
                {
                    Vector2Int coords = new(e, i);
                    _initialCellsDisposition.Add(coords, levelModel.LevelDisposition[index]);
                    index++;
                    Do(new(coords));
                }
            }
        }

        public void Do(GridCellModel gridCell)
        {
            int _blockKind = CheckHandPlacementData(gridCell.AnchorCoords);
            gridCell.BlockModel = new(_blockKind, gridCell.AnchorCoords);

            _model.GridObjects.Add(gridCell.AnchorCoords, _poolManager.SpawnBlockView(_blockKind, gridCell.AnchorCoords));
            _model.GridData.Add(gridCell.AnchorCoords, gridCell);
        }
        private int CheckHandPlacementData(Vector2Int cellCoords)
        {
            if (_initialCellsDisposition.TryGetValue(cellCoords, out int cellKindIndex) && cellKindIndex != 9)
                return cellKindIndex;

            int n = Random.Range(0, _config.GridBlocks.BaseBlocks.Count());
            return _config.GridBlocks.BaseBlocks[n].Id;
        }
    }
}