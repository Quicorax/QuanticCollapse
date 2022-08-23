using UnityEngine;

public class BlockModel
{
    public ElementKind Kind;
    public Vector2Int Coords;
    public GameObject View;

    public BaseBooster Booster;

    public int CollapseSteps;

    public bool IsBooster;
    public bool IsTriggered;

    public BlockModel(ElementKind kind, Vector2Int coords, GameObject objectView, bool isBooster = false, BaseBooster booster = null)
    {
        Kind = kind;
        Coords = coords;
        View = objectView;

        IsBooster = isBooster;
        Booster = booster;
    }
}
