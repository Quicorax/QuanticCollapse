using UnityEngine;
using DG.Tweening;

public class FloatingStarshipMovement : MonoBehaviour
{
    public float floatingDispersion;

    Vector3 intialPosition;

    void Start()
    {
        intialPosition = transform.position;
        InitFloatation();
    }

    void InitFloatation()
    {
        float rngY = Random.Range(-floatingDispersion, floatingDispersion);
        float rngX = Random.Range(-floatingDispersion, floatingDispersion);


        transform.DOLocalRotate(Vector3.forward * (rngX > 0 ? 2f : -2f), 2f);

        Vector3 punchFinalPosition = new Vector3(rngX, rngY, 0);

        //transform.DOPunchPosition(punchFinalPosition, 5f, 0, 1).SetEase(Ease.InOutSine).OnComplete(()=>InitFloatation());
        transform.DOMove(intialPosition + punchFinalPosition, 2f).SetEase(Ease.InOutSine).OnComplete(() => InitFloatation());
    }
}
