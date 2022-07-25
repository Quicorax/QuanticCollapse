using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGidCellWithInitialDispositionCommand : IGridCommand
{
    private PoolManager _poolManager;

    private Vector2 _coordsToFill;
    private ElementKind _blockKind;

    private Texture2D _initialDispositionTexture;

    private Color[] _colors =
{
        new Color(1,0,0,1),
        new Color(0,1,0,1),
        new Color(0,0,1,1),
        new Color(1,1,0,1)
    };

    public FillGidCellWithInitialDispositionCommand(PoolManager poolManager, Vector2 coordsToFill, Texture2D initialDispositionTexture)
    {
        _poolManager = poolManager;
        _coordsToFill = coordsToFill;
        _initialDispositionTexture = initialDispositionTexture;
    }
    public void Do(VirtualGridModel Model)
    {
        _blockKind = CheckHandPlacementData(_coordsToFill);

        Model.virtualGrid[_coordsToFill].SetDynamicBlockOnCell(new DynamicBlock(_blockKind, _coordsToFill, _poolManager.SpawnBlockView(_blockKind, _coordsToFill)));
    }
    ElementKind CheckHandPlacementData(Vector2 cellCoords)
    {
        Color pixelColor = _initialDispositionTexture.GetPixel((int)cellCoords.x, (int)cellCoords.y);
        for (int i = 0; i < _colors.Length; i++)
        {
            if (pixelColor == _colors[i])
                return (ElementKind)i;

        }
        return RandomElementKind();
    }

    ElementKind RandomElementKind()
    {
        return (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length - 1);
    }
}
