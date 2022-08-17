using UnityEngine;
using DG.Tweening;

public class BlackCircleTransition : MonoBehaviour
{
    [SerializeField] private Transform blackTransitionMask;

    public void TriggerCircleNarrow() { blackTransitionMask.DOScale(0, 1.9f).SetEase(Ease.InBack); }
}
