using UnityEngine;

public interface BaseBooster
{
    public int BoosterKindId { get; }
    public void OnInteraction(Vector2Int initialCoords, GridModel Model);
}
