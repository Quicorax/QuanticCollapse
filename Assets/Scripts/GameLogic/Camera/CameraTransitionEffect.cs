using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

namespace QuanticCollapse
{
    public class CameraTransitionEffect : MonoBehaviour
    {
        private float cameraOriginalZ;
        [SerializeField] private float cameraOffsetZ;
        private Volume _volume;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
        }
        private void Start()
        {
            cameraOriginalZ = Camera.main.transform.position.z;
            Camera.main.fieldOfView = 70;
            _volume.weight = 0;



        }

        public void CameraOnEngageEffect()
        {
            Camera.main.transform.DOMoveZ(cameraOriginalZ + cameraOffsetZ, 1f);
            Camera.main.DOFieldOfView(100, 1.5f);

            DOTween.To(() => _volume.weight, x => _volume.weight = x, 1, 1.8f).SetEase(Ease.InCirc);
        }

        public void CameraOnHangar(bool onHangar)
        {
            Camera.main.transform.DOMoveZ(onHangar ? -5 : -8, 1f);
            Camera.main.transform.DORotate(Vector3.right * (onHangar ? 50 : 30), 1f);
            Camera.main.DOFieldOfView(onHangar ? 80 : 70, 1.5f);
        }
        public void CameraOnShop(bool onShop)
        {
            Camera.main.transform.DOMove(onShop ? new Vector3(0, 10, -14) : new Vector3(0, 8, -8), 1f);
            Camera.main.transform.DORotate(Vector3.right * (onShop ? 15f : 30), 1f);
            Camera.main.DOFieldOfView(onShop ? 65 : 70, 1.5f);
        }
    }
}