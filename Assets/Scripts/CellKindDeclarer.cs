using UnityEngine;

public class CellKindDeclarer : MonoBehaviour
{
    public LevelGridData gridData;
    public ColorData colorData;
    public ElementKind CheckHandPlacementData(Vector2 cellCoords)
    {
        Color pixelColor = gridData.gridInitialLayout.GetPixel((int)cellCoords.x, (int)cellCoords.y);

        for (int i = 0; i < colorData.colors.Length; i++)
        {
            if (pixelColor == colorData.colors[i])
            {
                return (ElementKind)i;
            }
        }

        return RandomElementKind();
    }

    public ElementKind RandomElementKind() { return (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length); }
}
