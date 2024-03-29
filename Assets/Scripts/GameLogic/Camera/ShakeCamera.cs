using UnityEngine;

namespace QuanticCollapse
{
    public class ShakeCamera : MonoBehaviour
    {
        [SerializeField] private GenericEventBus _playerHitEventBus;
        [SerializeField] private CameraShakeData cameraShakeData;

        private void Awake()
        {
            _playerHitEventBus.Event += CameraShake;
        }
        private void OnDisable()
        {
            _playerHitEventBus.Event -= CameraShake;
        }
        public void CameraShake() { cameraShakeData.Shake(); }
    }
}