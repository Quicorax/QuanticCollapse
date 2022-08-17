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

    public void TriggerCameraTransitionEffect()
    {
        Camera.main.transform.DOMoveZ(cameraOriginalZ + cameraOffsetZ, 1f);
        Camera.main.DOFieldOfView(100, 1.5f);
    }
}
