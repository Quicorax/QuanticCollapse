using UnityEngine;

public class CellKindDeclarer : MonoBehaviour
{
    public LevelGridData gridData;
    public ColorData colorData;

    public ElementKind GetCellKind(bool initial, Vector2 coords) { return initial ? CheckHandPlacementData(coords) : RandomElementKind(); }

    ElementKind CheckHandPlacementData(Vector2 cellCoords)
    {
        Color pixelColor = gridData.gridInitialLayout.GetPixel((int)cellCoords.x, (int)cellCoords.y);

        if (colorData.CheckColorCoincidence(pixelColor, out int i))
            return (ElementKind)i;

        return RandomElementKind();
    }

    public ElementKind RandomElementKind() { return (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length -1); }
}
