using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterActionEventBus", menuName = "ScriptableObjects/EventBus/BoosterAction")]
public class BoosterActionEventBus : ScriptableObject
{
    public event Action<Vector2[]> Event = delegate(Vector2[] coords) { };
    public void NotifyEvent(Vector2[] coords) => Event?.Invoke(coords);
}
