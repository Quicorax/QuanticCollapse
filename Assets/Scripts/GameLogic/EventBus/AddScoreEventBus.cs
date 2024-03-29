using System;
using UnityEngine;

namespace QuanticCollapse
{
    [CreateAssetMenu(fileName = "AddScoreEventBus", menuName = "ScriptableObjects/EventBus/AddScore")]
    public class AddScoreEventBus : ScriptableObject
    {
        public event Action<int, int> Event = delegate (int kind, int amount) { };
        public void NotifyEvent(int kind, int amount) => Event?.Invoke(kind, amount);
    }
}