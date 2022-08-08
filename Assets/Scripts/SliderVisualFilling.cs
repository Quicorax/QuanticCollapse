using DG.Tweening;
using UnityEngine;

public class SliderVisualFilling : MonoBehaviour
{
    public void FillEffect()
    {
        transform.DOPunchScale(Vector2.one * .09f, 0.3f).SetEase(Ease.InOutElastic);
    }
}
