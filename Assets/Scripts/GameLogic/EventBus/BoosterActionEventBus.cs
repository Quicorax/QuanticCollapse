using System;
using UnityEngine;

namespace QuanticCollapse
{
    [CreateAssetMenu(fileName = "BoosterActionEventBus", menuName = "ScriptableObjects/EventBus/BoosterAction")]
    public class BoosterActionEventBus : ScriptableObject
    {
        public event Action<Vector2[]> Event = delegate { };
        public void NotifyEvent(Vector2[] coords) => Event?.Invoke(coords);
    }
}