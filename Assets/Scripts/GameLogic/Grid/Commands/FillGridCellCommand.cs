using DG.Tweening;
using System.Linq;
using UnityEngine;

public class FillGridCellCommand : IGridCommand
{
    private PoolManager _poolManager;

    private Vector2Int _coordsToFill;
    private int _blockId;
    public FillGridCellCommand(PoolManager poolManager, Vector2Int coordsToFill)
    {
        _poolManager = poolManager;
        _coordsToFill = coordsToFill;
    }
    public void Do(GridModel Model)
    {
        _blockId = GetRandom();
        Debug.Log(_blockId);
        GameObject newBlockView = _poolManager.SpawnBlockView(_blockId, new Vector2Int(_coordsToFill.x, 8));
        newBlockView.transform.DOMoveY(_coordsToFill.y, 0.4f).SetEase(Ease.OutBounce);

        Model.VirtualGrid[_coordsToFill].SetDynamicBlockOnCell(new CellBlockModel(_blockId, _coordsToFill, newBlockView));
    }

    int GetRandom() => Random.Range(0, ServiceLocator.GetService<GameConfigService>().GridBlocks.Where(item => !item.IsBooster).Count());
}
