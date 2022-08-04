using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpaceShaderMovementController : MonoBehaviour
{
    Material _spaceShader;

    [Range(-1f, 1f)] public float RelativeDirectionX;
    [Range(-1f, 1f)] public float RelativeDirectionY;

    private void Awake()
    {
        _spaceShader = GetComponent<MeshRenderer>().material;
    }
    void Start()
    {
        InitMovementSimulation();
    }

    void InitMovementSimulation()
    {
        _spaceShader.SetFloat("_X_SpaceRelativeMovement", RelativeDirectionX);
        _spaceShader.SetFloat("_Y_SpaceRelativeMovement", RelativeDirectionY);

    }
}
