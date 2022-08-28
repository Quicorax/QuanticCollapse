using UnityEngine;

public class GenerateInitialGridCellCommand : IGridCommand
{
    private PoolManager _poolManager;
    private Texture2D _initialDispositionTexture;
    private GridCellController _gridController;

    private Vector2Int _coords;

    private Color[] _colors =
{
        new Color(1,0,0,1),
        new Color(0,1,0,1),
        new Color(0,0,1,1),
        new Color(1,1,0,1)
    };

    public GenerateInitialGridCellCommand(PoolManager poolManager, Texture2D initialDispositionTexture, GridCellController gridCell, Vector2Int coords)
    {
        _poolManager = poolManager;
        _initialDispositionTexture = initialDispositionTexture;
        _gridController = gridCell;
        _coords = coords;
    }
    public void Do(VirtualGridModel Model)
    {
        ElementKind _blockKind = CheckHandPlacementData(_coords);
        _gridController.SetDynamicBlockOnCell(new BlockModel(_blockKind, _coords, _poolManager.SpawnBlockView(_blockKind, _coords)));

        Model.virtualGrid.Add(_coords, _gridController);
    }
    ElementKind CheckHandPlacementData(Vector2Int cellCoords)
    {
        Color pixelColor = _initialDispositionTexture.GetPixel(cellCoords.x, cellCoords.y);

        for (int color = 0; color < _colors.Length; color++)
            if (pixelColor == _colors[color])
                return (ElementKind)color;

        return (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length - 3);
    }
}
