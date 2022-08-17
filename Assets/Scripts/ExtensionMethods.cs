using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2[] GetCrossCoords(this Vector2 originVector)
    {
        Vector2[] crossNeighbours =
        {
            originVector + Vector2.left,
            originVector + Vector2.up,
            originVector + Vector2.right,
            originVector + Vector2.down,
        };

        return crossNeighbours;
    }
    public static Vector2[] GetSplashCoords(this Vector2 originVector)
    {
        Vector2[] splashNeighbours =
        {
            originVector + Vector2.left,
            originVector + Vector2.left * 2,
            originVector + Vector2.up,
            originVector + Vector2.up * 2,
            originVector + Vector2.up * 3,
            originVector + Vector2.right,
            originVector + Vector2.right * 2,
            originVector + Vector2.down,
            originVector + Vector2.down * 2,
            originVector + Vector2.up + Vector2.left,
            originVector + Vector2.up + Vector2.right,
            originVector + Vector2.down + Vector2.left,
            originVector + Vector2.down + Vector2.right,
        };

        return splashNeighbours;
    }
}
