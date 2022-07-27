using DG.Tweening;
using UnityEngine;

public class FillGridCellCommand : IGridCommand
{
    private PoolManager _poolManager;

    private Vector2 _coordsToFill;
    private ElementKind _blockKind;
    public FillGridCellCommand(PoolManager poolManager, Vector2 coordsToFill)
    {
        _poolManager = poolManager;
        _coordsToFill = coordsToFill;
    }
    public void Do(VirtualGridModel Model)
    {
        _blockKind = GetRandom();
        GameObject newBlockView = _poolManager.SpawnBlockView(_blockKind, new Vector2(_coordsToFill.x, 8));
        newBlockView.transform.DOMoveY(_coordsToFill.y, 0.4f).SetEase(Ease.OutBounce);

        Model.virtualGrid[_coordsToFill].SetDynamicBlockOnCell(new DynamicBlock(_blockKind, _coordsToFill, newBlockView));
    }

    ElementKind GetRandom()
    {
        return (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length - 3);
    }
}
