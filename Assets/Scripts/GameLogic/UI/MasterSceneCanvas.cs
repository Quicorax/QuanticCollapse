using DG.Tweening;
using UnityEngine;

public class MasterSceneCanvas : MonoBehaviour
{
    private CanvasGroup CanvasGroup;
    private Transform IconTransform;

    private bool pause;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        IconTransform = transform.GetChild(0);
    }
    private void Start()
    {
        IconInitMovement();
    }
    public void FadeCanvas(bool fade)
    {
        CanvasGroup.DOFade(fade ? 0 : 1, 0.5f);

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

        IconTransform.DORotate(Vector2.up * 360, 1f, RotateMode.LocalAxisAdd)
            .OnComplete(() => IconRotate());
    }
}
