using System;
using UnityEngine;

namespace QuanticCollapse
{
    [CreateAssetMenu(fileName = "LevelInjectedEventBus", menuName = "ScriptableObjects/EventBus/LevelInjected")]
    public class LevelInjectedEventBus : ScriptableObject
    {
        public event Action<LevelModel> Event = delegate (LevelModel levelData) { };
        public void NotifyEvent(LevelModel levelData) => Event?.Invoke(levelData);
    }
}