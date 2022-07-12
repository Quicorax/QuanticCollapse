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

    //Use that to call the extension:
    //return ExtensionMethods.GetRandomElementKind<ElementKind>();

    //public static T GetRandomElementKind<T>() where T : struct, System.IConvertible
    //{
    //    List<T> genericList = new();
    //    foreach (T item in System.Enum.GetValues(typeof(T)))
    //        genericList.Add(item);
    //
    //    return genericList[Random.Range(0, genericList.Count)];
    //}
}
