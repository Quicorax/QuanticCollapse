using System;
using UnityEngine;
[CreateAssetMenu(fileName = "StarshipModuleActivationEventBus", menuName = "ScriptableObjects/EventBus/StarshipModuleActivation")]
public class StarshipModuleActivationEventBus : ScriptableObject
{
    public event Action<bool, int, int> Event = delegate (bool player, int kind, int force) { };
    public void NotifyEvent(bool player, int kind, int force) => Event?.Invoke(player, kind, force);
}
