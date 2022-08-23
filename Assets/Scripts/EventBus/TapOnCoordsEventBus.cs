using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TapOnCoordsEventBus", menuName = "ScriptableObjects/EventBus/TapOnCoords")]
public class TapOnCoordsEventBus : ScriptableObject
{
    public event Action<Vector2Int, bool> Event = delegate (Vector2Int coords, bool boostedInput) { };
    public void NotifyEvent(Vector2Int coords, bool boostedInput) => Event?.Invoke(coords, boostedInput);
}
