using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShaderMovementController : MonoBehaviour
{
    public Material spaceMaterial;

    float RollRotation;
    float PitchRotation;
    void Start()
    {
        MovementSimulationValueChanged();
    }
    private void OnDisable()
    {
        spaceMaterial.SetFloat("_RollRelativeMovement", 0);
    }

    void MovementSimulationValueChanged()
    {
        RollRotation = Random.Range(-0.7f, 0.7f);
        PitchRotation = Random.Range(-0.7f, 0.7f);

        spaceMaterial.SetFloat("_RollRelativeMovement", RollRotation);
        spaceMaterial.SetFloat("_PitchRelativeMovement", PitchRotation);
    }
}
