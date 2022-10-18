using System;
using UnityEngine;

namespace QuanticCollapse
{
    [CreateAssetMenu(fileName = "SendSceneTransitionerReference", menuName = "ScriptableObjects/EventBus/SendSceneTransitionerReference")]
    public class SendSceneTransitionerReferenceEventBus : ScriptableObject
    {
        public event Action<SceneTransitioner> Event = delegate (SceneTransitioner transitioner) { };
        public void NotifyEvent(SceneTransitioner transitioner) => Event?.Invoke(transitioner);
    }
}