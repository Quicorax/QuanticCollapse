using UnityEngine;

public class CellKindDeclarer : MonoBehaviour
{
    public LevelGridData gridData;

    private Color[] colors =
    {
        new Color(1,0,0,1), 
        new Color(0,1,0,1), 
        new Color(0,0,1,1), 
        new Color(1,1,0,1)  
    };

    public ElementKind GetCellKind(bool initial, Vector2 coords) { return initial ? CheckHandPlacementData(coords) : RandomElementKind(); }

    ElementKind CheckHandPlacementData(Vector2 cellCoords)
    {
        Color pixelColor = gridData.gridInitialLayout.GetPixel((int)cellCoords.x, (int)cellCoords.y);
        for (int i = 0; i < colors.Length; i++)
        {
            if(pixelColor == colors[i])
                return (ElementKind)i;

        }
        return RandomElementKind();
    }

    public ElementKind RandomElementKind() { return (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length -1); }
}
