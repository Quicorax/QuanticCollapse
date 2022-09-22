using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ExternalBoosterScreenEffect", menuName = "ScriptableObjects/EventBus/ExternalBoosterScreenEffect")]
public class ExternalBoosterScreenEffectEventBus : ScriptableObject
{
    public event Action<ResourcesType> Event = delegate (ResourcesType s) { };
    public void NotifyEvent(ResourcesType s) => Event?.Invoke(s);
}
