using DG.Tweening;
using UnityEngine;

public class FillGridCellCommand : IGridCommand
{
    private PoolManager _poolManager;

    private Vector2Int _coordsToFill;
    private ElementKind _blockKind;
    public FillGridCellCommand(PoolManager poolManager, Vector2Int coordsToFill)
    {
        _poolManager = poolManager;
        _coordsToFill = coordsToFill;
    }
    public void Do(GridModel Model)
    {
        _blockKind = GetRandom();
        GameObject newBlockView = _poolManager.SpawnBlockView(_blockKind, new Vector2Int(_coordsToFill.x, 8));
        newBlockView.transform.DOMoveY(_coordsToFill.y, 0.4f).SetEase(Ease.OutBounce);

        Model.VirtualGrid[_coordsToFill].SetDynamicBlockOnCell(new BlockModel(_blockKind, _coordsToFill, newBlockView));
    }

    ElementKind GetRandom()
    {
        return (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length - 3);
    }
}
