using UnityEngine;
using Cinemachine;


namespace QuanticCollapse
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CameraShakeData")]
    public class CameraShakeData : ScriptableObject
    {
        [SerializeField] CinemachineImpulseSource _cameraShakeSource;
        public void Shake() => _cameraShakeSource.GenerateImpulse();
    }
}