using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericEventBus", menuName = "ScriptableObjects/EventBus/Generic")]
public class GenericEventBus : ScriptableObject
{
    public event Action Event = delegate () { };
    public void NotifyEvent() => Event?.Invoke();
}
