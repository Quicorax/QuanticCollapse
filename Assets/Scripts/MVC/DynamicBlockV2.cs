using UnityEngine;

public class DynamicBlockV2
{
    public ElementKind blockKind;
    public Vector2 blockCoords;
    public GameObject blockView;

    public bool isBooster;
    public BaseBooster booster;


    public bool isHot;
    public int collapseSteps;

    public DynamicBlockV2(ElementKind kind, Vector2 coords, GameObject objectView, bool isBooster = false, BaseBooster booster = null)
    {
        blockKind = kind;
        blockCoords = coords;
        blockView = objectView;

        this.isBooster = isBooster;
        this.booster = booster;
    }
}
