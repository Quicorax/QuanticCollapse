using System.Threading.Tasks;
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

    public static Color[] GenerateColorsFromHexFormatedString(this Color _, string formatedString)
    {
        string[] colorSplitString = formatedString.Split("_");

        ColorUtility.TryParseHtmlString(colorSplitString[0], out Color primaryColor);
        ColorUtility.TryParseHtmlString(colorSplitString[1], out Color secondaryColor);
        ColorUtility.TryParseHtmlString(colorSplitString[2], out Color signatureColor);

        Color[] resultColorPack =
        {
            primaryColor,
            secondaryColor,
            signatureColor
        };

        return resultColorPack;
    }
    public static void ManageTaskExeption(this Task task)
    {
        task.ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
    }
}
