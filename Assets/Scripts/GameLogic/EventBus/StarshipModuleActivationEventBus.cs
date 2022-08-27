using System;
using UnityEngine;
[CreateAssetMenu(fileName = "StarshipModuleActivationEventBus", menuName = "ScriptableObjects/EventBus/StarshipModuleActivation")]
public class StarshipModuleActivationEventBus : ScriptableObject
{
    public event Action<bool, ElementKind, int> Event = delegate (bool player, ElementKind kind, int force) { };
    public void NotifyEvent(bool player, ElementKind kind, int force) => Event?.Invoke(player, kind, force);
}
