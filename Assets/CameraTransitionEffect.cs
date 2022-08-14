using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraTransitionEffect : MonoBehaviour
{
    Camera camera;
    float cameraOriginalZ;
    public float cameraOffsetZ;

    private void Awake()
    {
        camera = Camera.main;
    }
    private void Start()
    {
        cameraOriginalZ = camera.transform.position.z;
        camera.fieldOfView = 70;
    }

    public void TriggerCameraTransitionEffect()
    {
        camera.transform.DOMoveZ(cameraOriginalZ + cameraOffsetZ, 1f);
        camera.DOFieldOfView(100, 1.5f);
    }
}
