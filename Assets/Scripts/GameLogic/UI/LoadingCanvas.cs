using DG.Tweening;
using UnityEngine;

public class LoadingCanvas : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Transform _iconTransform;

    private bool pause;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _iconTransform = transform.GetChild(0);
    }
    private void Start()
    {
        IconInitMovement();
    }
    public void FadeCanvas(bool fade)
    {
        _canvasGroup.DOFade(fade ? 0 : 1, 0.5f);

        if (fade)
            IconPauseRotation();
        else
            IconInitMovement();
    }
    void IconInitMovement()
    {
        pause = false;
        IconRotate();
    }
    void IconPauseRotation()
    {
        pause = true;
    }
    void IconRotate()
    {
        if (pause)
            return;

        _iconTransform.DORotate(Vector2.up * 360, 1f, RotateMode.LocalAxisAdd)
            .OnComplete(() => IconRotate());
    }
}
