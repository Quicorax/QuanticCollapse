using UnityEngine;

public interface BaseBooster
{
    public ElementKind BoosterKind { get; }
    public void OnInteraction(Vector2Int initialCoords, GridController Controller);
}
