using System;
using UnityEngine;

namespace QuanticCollapse
{
    [CreateAssetMenu(fileName = "ExternalBoosterScreenEffect",
        menuName = "ScriptableObjects/EventBus/ExternalBoosterScreenEffect")]
    public class ExternalBoosterScreenEffectEventBus : ScriptableObject
    {
        public event Action<string> Event = delegate { };
        public void NotifyEvent(string s) => Event?.Invoke(s);
    }
}