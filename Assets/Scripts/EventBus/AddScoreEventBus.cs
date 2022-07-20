using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddScoreEventBus", menuName = "ScriptableObjects/EventBus/AddScore")]
public class AddScoreEventBus : ScriptableObject
{
    public event Action<ElementKind, int> Event = delegate (ElementKind kind, int amount) { };
    public void NotifyEvent(ElementKind kind, int amount) => Event?.Invoke(kind, amount);
}
