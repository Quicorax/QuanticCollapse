using DG.Tweening;
using UnityEngine;

namespace QuanticCollapse
{
    public class LoadingCanvas : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Transform _iconTransform;

        private bool _pause;

        public void FadeCanvas(bool fade)
        {
            _canvasGroup.DOFade(fade ? 0 : 1, 0.5f);

            if (fade)
            {
                IconPauseRotation();
            }
            else
            {
                IconInitMovement();
            }
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _iconTransform = transform.GetChild(0);
        }

        private void Start()
        {
            IconInitMovement();
        }

        private void IconInitMovement()
        {
            _pause = false;
            IconRotate();
        }

        private void IconPauseRotation()
        {
            _pause = true;
        }

        private void IconRotate()
        {
            if (_pause)
                return;

            _iconTransform.DORotate(Vector2.up * 360, 1f, RotateMode.LocalAxisAdd)
                .OnComplete(IconRotate);
        }
    }
}