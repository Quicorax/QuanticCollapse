using UnityEngine;
using DG.Tweening;


public class StarshipAnimationController : MonoBehaviour
{
    [SerializeField] private float floatingDispersion;

    private bool transitioning;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;

        InitFloatation();
    }
    void InitFloatation()
    {
        if (transitioning)
            return;

        float rngY = Random.Range(-floatingDispersion, floatingDispersion);
        float rngX = Random.Range(-floatingDispersion, floatingDispersion);
        float rngZ = Random.Range(-floatingDispersion, floatingDispersion);

        transform.DOLocalRotate(Vector3.forward * (rngX > 0 ? 5f : -5f), 2f);
        transform.DOMove(initialPosition + new Vector3(rngX, rngY, rngZ), 2f).SetEase(Ease.InOutSine).OnComplete(() => InitFloatation());
    }

    public void TriggerTransitionAnimation()
    {
        transitioning = true;

        transform.DOLocalRotate(Vector3.zero, 1);
        transform.DOMoveZ(transform.position.z + 70, 2f).SetEase(Ease.InBack);
        transform.DOScale(0,2f).SetEase(Ease.InExpo);
    }
}
