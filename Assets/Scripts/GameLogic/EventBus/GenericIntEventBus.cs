using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericIntEventBus", menuName = "ScriptableObjects/EventBus/GenericInt")]
public class GenericIntEventBus : ScriptableObject
{
    public event Action<int> Event = delegate { };
    public void NotifyEvent(int i) => Event?.Invoke(i);
}