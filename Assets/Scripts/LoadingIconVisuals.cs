using DG.Tweening;
using UnityEngine;

public class LoadingIconVisuals : MonoBehaviour
{
    bool pause;
    void Start()
    {
        Rotate();
    }

    public void Init()
    {
        pause = false;
        Rotate();
    }
    public void Pause()
    {
        pause = true;
    }
    void Rotate()
    {
        if (pause)
            return;

        transform.DORotate(Vector2.up * 360, 1f, RotateMode.LocalAxisAdd)
            .OnComplete(() => Rotate());
    }
}
