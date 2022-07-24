using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TapOnCoordsEventBus", menuName = "ScriptableObjects/EventBus/TapOnCoords")]
public class TapOnCoordsEventBus : ScriptableObject
{
    public event Action<Vector2> Event = delegate (Vector2 coords) { };
    public void NotifyEvent(Vector2 coords) => Event?.Invoke(coords);
}
