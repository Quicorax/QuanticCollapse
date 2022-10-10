using DG.Tweening;
using System.Linq;
using UnityEngine;

public class FillGridCellCommand : IGridCommand
{
    private GameConfigService _config;
    private PoolManager _poolManager;

    private Vector2Int _coordsToFill;
    private int _blockId;
    public FillGridCellCommand(PoolManager poolManager, Vector2Int coordsToFill)
    {
        _poolManager = poolManager;
        _coordsToFill = coordsToFill;
        _config = ServiceLocator.GetService<GameConfigService>();
    }
    public void Do(GridModel Model)
    {
        _blockId = GetRandom();
        GameObject newBlockView = _poolManager.SpawnBlockView(_blockId, new Vector2Int(_coordsToFill.x, 8));
        newBlockView.transform.DOMoveY(_coordsToFill.y, 0.4f).SetEase(Ease.OutBounce);

        Model.VirtualGrid[_coordsToFill].SetDynamicBlockOnCell(new CellBlockModel(_blockId, _coordsToFill, newBlockView));
    }

    int GetRandom() 
    {
        int n = Random.Range(0, _config.GridBlocks.BaseBlocks.Count());
        return _config.GridBlocks.BaseBlocks[n].Id;
    }
}
