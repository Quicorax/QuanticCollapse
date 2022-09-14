using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SendMasterReference", menuName = "ScriptableObjects/EventBus/SendMasterReference")]
public class SendMasterReferenceEventBus : ScriptableObject
{
    public event Action<MasterSceneTransitioner> Event = delegate (MasterSceneTransitioner master) { };
    public void NotifyEvent(MasterSceneTransitioner master) => Event?.Invoke(master);
}
