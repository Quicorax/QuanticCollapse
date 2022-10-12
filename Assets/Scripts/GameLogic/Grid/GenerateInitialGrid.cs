using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateInitialGrid 
{
    private PoolManager _poolManager;
    private GridCellModel _gridCellModel;
    private GameConfigService _config;
    private Dictionary<Vector2Int, int> _initialCellsDisposition = new();
    public GenerateInitialGrid(PoolManager poolManager, LevelModel levelModel, GridCellModel gridCell)
    {
        _poolManager = poolManager;
        _gridCellModel = gridCell;
        _config = ServiceLocator.GetService<GameConfigService>();

        Initialize(levelModel);
    }

    public void Do(GridModel Model)
    {
        int _blockKind = CheckHandPlacementData(_gridCellModel.AnchorCoords);
        _gridCellModel.BlockModel = new(_blockKind, _gridCellModel.AnchorCoords);

        Model.GridObjects.Add(_gridCellModel.AnchorCoords, _poolManager.SpawnBlockView(_blockKind, _gridCellModel.AnchorCoords));
        Model.GridData.Add(_gridCellModel.AnchorCoords, _gridCellModel);
    }
    void Initialize(LevelModel levelModel)
    {
        int index = 0;

        for (int i = 0; i < levelModel.LevelHeight; i++)
        {
            for (int e = 0; e < levelModel.LevelWidth; e++)
            {
                _initialCellsDisposition.Add(new Vector2Int(e, i), levelModel.LevelDisposition[index]);
                index++;
            }
        }
    }
    int CheckHandPlacementData(Vector2Int cellCoords)
    {
        if (_initialCellsDisposition.TryGetValue(cellCoords, out int cellKindIndex) && cellKindIndex != 9)
            return cellKindIndex;

        int n = Random.Range(0, _config.GridBlocks.BaseBlocks.Count());
        return _config.GridBlocks.BaseBlocks[n].Id;
    }
}
