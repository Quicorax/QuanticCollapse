using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateInitialGridCellCommand : IGridCommand
{
    private PoolManager _poolManager;
    private GridCellController _gridCellController;
    private Dictionary<Vector2Int, int> _initialCellsDisposition = new();
    private GameConfigService _config;
    public GenerateInitialGridCellCommand(PoolManager poolManager, LevelModel levelModel, GridCellController gridCell)
    {
        _poolManager = poolManager;
        _gridCellController = gridCell;
        _config = ServiceLocator.GetService<GameConfigService>();

        Initialize(levelModel);
    }

    public void Do(GridModel Model)
    {
        int _blockKind = CheckHandPlacementData(_gridCellController.AnchorCoords);
        _gridCellController.SetDynamicBlockOnCell(new CellBlockModel(_blockKind, _gridCellController.AnchorCoords));

        Model.GridObjects.Add(_gridCellController.AnchorCoords, _poolManager.SpawnBlockView(_blockKind, _gridCellController.AnchorCoords));
        Model.GridData.Add(_gridCellController.AnchorCoords, _gridCellController);
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
