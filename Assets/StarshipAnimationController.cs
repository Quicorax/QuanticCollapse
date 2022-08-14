using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class StarshipAnimationController : MonoBehaviour
{
    public void TriggerTransitionAnimation()
    {
        transform.DOMoveZ(transform.position.z + 70, 2f).SetEase(Ease.InBack);
        transform.DOScale(0,2f).SetEase(Ease.InExpo);
    }
}
