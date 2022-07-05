using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2[] GetCrossCoords(this Vector2 originVector)
    {
        Vector2[] crossNeighbours =
        {
            originVector + Vector2.right,
            originVector + Vector2.left,
            originVector + Vector2.up,
            originVector + Vector2.down
        };

        return crossNeighbours;
    }
}
