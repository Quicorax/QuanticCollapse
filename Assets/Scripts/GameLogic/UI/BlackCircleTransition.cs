using UnityEngine;
using DG.Tweening;

namespace QuanticCollapse
{
    public class BlackCircleTransition : MonoBehaviour
    {
        [SerializeField] private Transform blackTransitionMask;

        public void TriggerCircleToClose() => blackTransitionMask.DOScale(0, 1.9f).SetEase(Ease.InBack);
        private void TriggerCircleToOpen() => blackTransitionMask.DOScale(1.5f, 2f).SetEase(Ease.OutCubic);

        private void Start()
        {
            TriggerCircleToOpen();
        }
    }
}