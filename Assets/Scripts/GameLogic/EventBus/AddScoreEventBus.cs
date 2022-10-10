using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AddScoreEventBus", menuName = "ScriptableObjects/EventBus/AddScore")]
public class AddScoreEventBus : ScriptableObject
{
    public event Action<int, int> Event = delegate (int kind, int amount) { };
    public void NotifyEvent(int kind, int amount) => Event?.Invoke(kind, amount);
}
