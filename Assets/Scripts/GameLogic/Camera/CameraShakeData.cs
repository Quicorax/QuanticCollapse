﻿using UnityEngine;
using Cinemachine;


namespace QuanticCollapse
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CameraShakeData")]
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class CameraShakeData : ScriptableObject
    {
        public CinemachineImpulseSource cameraShakeSource;
        public void Shake() { cameraShakeSource.GenerateImpulse(); }
    }
}