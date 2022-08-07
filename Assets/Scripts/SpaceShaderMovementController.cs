using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShaderMovementController : MonoBehaviour
{
    public Material[] dynamicShaders;

    [Range(-1f, 1f)] public float RollRotation;
    [Range(-1f, 1f)] public float PitchRotation;
    void Start()
    {
        MovementSimulationValueChanged();
        //DynamicRelativeMovement();
    }
    private void OnDisable()
    {
        for (int i = 0; i < dynamicShaders.Length; i++)
        {
            dynamicShaders[i].SetFloat("_RollRelativeMovement", 0);
            dynamicShaders[i].SetFloat("_PitchRelativeMovement", 0);
        }
    }

    //float lastRoll;
    //float lastPitch;
    //private void Update()
    //{
    //    if (RollRotation != lastRoll || PitchRotation != lastPitch)
    //    {
    //        lastRoll = RollRotation;
    //        lastPitch = PitchRotation;
    //
    //        MovementSimulationValueChanged();
    //    }
    //}


    void MovementSimulationValueChanged()
    {
        RollRotation = Random.Range(-0.7f, 0.7f);
        PitchRotation = Random.Range(-0.7f, 0.7f);

        for (int i = 0; i < dynamicShaders.Length; i++)
        {
            dynamicShaders[i].SetFloat("_RollRelativeMovement", RollRotation);
            dynamicShaders[i].SetFloat("_PitchRelativeMovement", PitchRotation);
        }

        float inBetweenTime = Random.Range(1f, 5f);

        Invoke(nameof(MovementSimulationValueChanged), inBetweenTime);
    }


    void DynamicRelativeMovement() 
    {
        RollRotation = Random.Range(-.7f, .7f);
        PitchRotation = Random.Range(-.7f, .7f);

        for (int i = 0; i < dynamicShaders.Length; i++)
        {
            DOTween.Sequence()
                .Join(dynamicShaders[i].DOFloat(RollRotation, "_RollRelativeMovement", 3.5f).SetEase(Ease.Linear))
                .Join(dynamicShaders[i].DOFloat(PitchRotation, "_PitchRelativeMovement", 5f).SetEase(Ease.Linear))
                .OnComplete( ()=> DynamicRelativeMovement());
        }
    }
}
