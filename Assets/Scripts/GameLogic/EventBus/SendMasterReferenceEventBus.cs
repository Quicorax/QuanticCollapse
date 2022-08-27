using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SendMasterReference", menuName = "ScriptableObjects/EventBus/SendMasterReference")]
public class SendMasterReferenceEventBus : ScriptableObject
{
    public event Action<MasterSceneManager> Event = delegate (MasterSceneManager master) { };
    public void NotifyEvent(MasterSceneManager master) => Event?.Invoke(master);
}
