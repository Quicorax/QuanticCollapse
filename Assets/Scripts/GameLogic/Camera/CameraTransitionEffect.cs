using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

namespace QuanticCollapse
{
    public class CameraTransitionEffect : MonoBehaviour
    {
        [SerializeField] private float _cameraOffsetZ = -2;

        private Volume _volume;
        private Camera _camera;

        private float _cameraOriginalZ;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
        }

        private void Start()
        {
            _camera = Camera.main;

            _cameraOriginalZ = _camera.transform.position.z;
            _camera.fieldOfView = 70;
            _volume.weight = 0;
        }

        public void CameraOnEngageEffect()
        {
            _camera.transform.DOMoveZ(_cameraOriginalZ + _cameraOffsetZ, 1f);
            _camera.DOFieldOfView(100, 1.5f);

            DOTween.To(() => _volume.weight, x => _volume.weight = x, 1, 1.8f).SetEase(Ease.InCirc);
        }

        public void CameraOnHangar(bool onHangar)
        {
            _camera.transform.DOMoveZ(onHangar ? -5 : -8, 1f);
            _camera.transform.DORotate(Vector3.right * (onHangar ? 50 : 30), 1f);
            _camera.DOFieldOfView(onHangar ? 80 : 70, 1.5f);
        }

        public void CameraOnShop(bool onShop)
        {
            _camera.transform.DOMove(onShop ? new Vector3(0, 10, -14) : new Vector3(0, 8, -8), 1f);
            _camera.transform.DORotate(Vector3.right * (onShop ? 15f : 30), 1f);
            _camera.DOFieldOfView(onShop ? 65 : 70, 1.5f);
        }
    }
}