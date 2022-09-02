using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2Int[] GetCrossCoords(this Vector2Int originVector)
    {
        Vector2Int[] crossNeighbours =
        {
            originVector + Vector2Int.left,
            originVector + Vector2Int.up,
            originVector + Vector2Int.right,
            originVector + Vector2Int.down,
        };

        return crossNeighbours;
    }
    public static Vector2Int[] GetSplashCoords(this Vector2Int originVector)
    {
        Vector2Int[] splashNeighbours =
        {
            originVector + Vector2Int.left,
            originVector + Vector2Int.left * 2,
            originVector + Vector2Int.up,
            originVector + Vector2Int.up * 2,
            originVector + Vector2Int.up * 3,
            originVector + Vector2Int.right,
            originVector + Vector2Int.right * 2,
            originVector + Vector2Int.down,
            originVector + Vector2Int.down * 2,
            originVector + Vector2Int.up + Vector2Int.left,
            originVector + Vector2Int.up + Vector2Int.right,
            originVector + Vector2Int.down + Vector2Int.left,
            originVector + Vector2Int.down + Vector2Int.right,
        };

        return splashNeighbours;
    }

    public static Color[] GenerateColorPackFromFormatedString(this Color color, string formatedString)
    {
        string[] colorSplitString = formatedString.Split("_");

        string[] channelPrimarySplitString = colorSplitString[0].Split("-");
        string[] channelSecondarySplitString = colorSplitString[1].Split("-");
        string[] channelSignatureSplitString = colorSplitString[2].Split("-");

        Color[] resultColorPack = 
        {
            new Color(float.Parse(channelPrimarySplitString[0]),     float.Parse(channelPrimarySplitString[1]),     float.Parse(channelPrimarySplitString[2])),
            new Color(float.Parse(channelSecondarySplitString[0]),   float.Parse(channelSecondarySplitString[1]),   float.Parse(channelSecondarySplitString[2])),
            new Color(float.Parse(channelSignatureSplitString[0]),   float.Parse(channelSignatureSplitString[1]),   float.Parse(channelSignatureSplitString[2])),
        };

        return resultColorPack;
    }
}
