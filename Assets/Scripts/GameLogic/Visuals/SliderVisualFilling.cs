using DG.Tweening;
using UnityEngine;

namespace QuanticCollapse
{
    namespace QuanticCollapse
    {
        public class SliderVisualFilling : MonoBehaviour
        {
            private bool _animating;
            public void FillEffect() 
            {
                if (_animating)
                    return;

                _animating = true;
                transform.DOPunchScale(Vector2.one * .09f, 0.3f).SetEase(Ease.InOutElastic).OnComplete(() => 
                {
                    _animating = false;
                });
            }
        }
    }
}