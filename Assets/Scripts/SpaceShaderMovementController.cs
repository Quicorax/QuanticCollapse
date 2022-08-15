using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SpaceShaderMovementController : MonoBehaviour
{
    public Material spaceMaterial;

    //float RollRotation;
    //float PitchRotation;
    void Start()
    {
        //MovementSimulationValueChanged();
        StartCoroutine(SimulateEnemyLock());
    }
    IEnumerator SimulateEnemyLock()
    {
        spaceMaterial.SetFloat("_SpaceRelativeMovementSpeed", 3);
        yield return new WaitForSeconds(2.5f);
        spaceMaterial.DOFloat(1f, "_SpaceRelativeMovementSpeed", 0.5f);
    }
    //private void OnDisable()
    //{
    //    spaceMaterial.SetFloat("_RollRelativeMovement", 0);
    //}
    //
    //void MovementSimulationValueChanged()
    //{
    //    RollRotation = Random.Range(-0.7f, 0.7f);
    //    PitchRotation = Random.Range(-0.7f, 0.7f);
    //
    //    spaceMaterial.SetFloat("_RollRelativeMovement", RollRotation);
    //    spaceMaterial.SetFloat("_PitchRelativeMovement", PitchRotation);
    //}
}
