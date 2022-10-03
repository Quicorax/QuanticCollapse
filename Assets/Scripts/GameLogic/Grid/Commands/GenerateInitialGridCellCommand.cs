using System.Collections.Generic;
using UnityEngine;

public class GenerateInitialGridCellCommand : IGridCommand
{
    private PoolManager _poolManager;
    private GridCellController _gridCellController;
    private Dictionary<Vector2Int, int> _initialCellsDisposition = new();
    public GenerateInitialGridCellCommand(PoolManager poolManager, LevelModel levelModel, GridCellController gridCell)
    {
        _poolManager = poolManager;
        _gridCellController = gridCell;

        Initialize(levelModel);
    }

    public void Do(GridModel Model)
    {
        ElementKind _blockKind = CheckHandPlacementData(_gridCellController.AnchorCoords);
        _gridCellController.SetDynamicBlockOnCell(new BlockModel(
                _blockKind, 
                _gridCellController.AnchorCoords, 
                _poolManager.SpawnBlockView(_blockKind, _gridCellController.AnchorCoords)));

        Model.VirtualGrid.Add(_gridCellController.AnchorCoords, _gridCellController);
    }
    void Initialize(LevelModel levelModel)
    {
        string[] intDisp = levelModel.LevelDisposition.Split("-");
        int index = 0;

        for (int i = 0; i < levelModel.LevelHeight; i++)
        {
            for (int e = 0; e < levelModel.LevelWidth; e++)
            {
                _initialCellsDisposition.Add(new Vector2Int(e, i), int.Parse(intDisp[index]));
                index++;
            }
        }
    }
    ElementKind CheckHandPlacementData(Vector2Int cellCoords)
    {
        if (_initialCellsDisposition.TryGetValue(cellCoords, out int cellKindIndex) && cellKindIndex != 9)
            return (ElementKind)cellKindIndex;

        return (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length - 3);
    }
}
