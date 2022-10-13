using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace QuanticCollapse
{
    public class SpaceShaderMovementController : MonoBehaviour
    {
        [SerializeField] private Material spaceMaterial;

        void Start()
        {
            StartCoroutine(SimulateEnemyLock());
        }
        IEnumerator SimulateEnemyLock()
        {
            spaceMaterial.SetFloat("_SpaceRelativeMovementSpeed", 3);
            yield return new WaitForSeconds(2.5f);
            spaceMaterial.DOFloat(1f, "_SpaceRelativeMovementSpeed", 0.5f);
        }
    }
}