using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ExternalBoosterScreenEffect", menuName = "ScriptableObjects/EventBus/ExternalBoosterScreenEffect")]
public class ExternalBoosterScreenEffectEventBus : ScriptableObject
{
    public event Action<string> Event = delegate (string s) { };
    public void NotifyEvent(string s) => Event?.Invoke(s);
}
