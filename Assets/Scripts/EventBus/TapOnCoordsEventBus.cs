using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TapOnCoordsEventBus", menuName = "ScriptableObjects/EventBus/TapOnCoords")]
public class TapOnCoordsEventBus : ScriptableObject
{
    public event Action<Vector2, bool> Event = delegate (Vector2 coords, bool boostedInput) { };
    public void NotifyEvent(Vector2 coords, bool boostedInput) => Event?.Invoke(coords, boostedInput);
}
