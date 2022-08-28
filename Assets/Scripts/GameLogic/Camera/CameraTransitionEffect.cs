using UnityEngine;
using DG.Tweening;

public class CameraTransitionEffect : MonoBehaviour
{
    private float cameraOriginalZ;
    [SerializeField] private float cameraOffsetZ;

    private void Start()
    {
        cameraOriginalZ = Camera.main.transform.position.z;
        Camera.main.fieldOfView = 70;
    }

    public void CameraOnEngageEffect()
    {
        Camera.main.transform.DOMoveZ(cameraOriginalZ + cameraOffsetZ, 1f);
        Camera.main.DOFieldOfView(100, 1.5f);
    }

    public void CameraOnHangar(bool onHangar)
    {
        Camera.main.transform.DOMoveZ(onHangar ? 3 : -3, 1f).SetRelative();
        Camera.main.transform.DORotate(Vector3.right * (onHangar ? 50 : 30), 1f);
        Camera.main.DOFieldOfView(onHangar ? 80 : 70, 1.5f);
    }
}
