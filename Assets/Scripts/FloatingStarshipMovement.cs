using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingStarshipMovement : MonoBehaviour
{
    public float floatingDispersion;
    void Start()
    {
        InitFloatation();
    }

    void InitFloatation()
    {
        float rngY = Random.Range(-floatingDispersion, floatingDispersion);
        float rngX = Random.Range(-floatingDispersion, floatingDispersion);
        Vector2 punchFinalPosition = new Vector2(rngX, rngY);

        transform.DOPunchPosition(punchFinalPosition, 5f, 0, 1).SetEase(Ease.InOutSine).OnComplete(()=>InitFloatation());

                //transform.DOJump(transform.position, rngX, 1, 2f)
                //transform.DOJump(transform.position, rngY, 1, 2f)
    }
}
