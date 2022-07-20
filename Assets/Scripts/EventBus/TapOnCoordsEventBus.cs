using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TapOnCoordsEventBus", menuName = "ScriptableObjects/EventBus/TapOnCoords")]
public class TapOnCoordsEventBus : ScriptableObject
{
    public event Action<Vector2, bool> Event = delegate (Vector2 coords, bool isExternalBoosterInput) { };
    public void NotifyEvent(Vector2 coords, bool isExternalBoosterInput) => Event?.Invoke(coords, isExternalBoosterInput);
}
