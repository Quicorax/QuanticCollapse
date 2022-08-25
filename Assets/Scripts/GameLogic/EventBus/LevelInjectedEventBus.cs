using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInjectedEventBus", menuName = "ScriptableObjects/EventBus/LevelInjected")]
public class LevelInjectedEventBus : ScriptableObject
{
    public event Action<LevelGridData> Event = delegate (LevelGridData levelData) { };
    public void NotifyEvent(LevelGridData levelData) => Event?.Invoke(levelData);
}