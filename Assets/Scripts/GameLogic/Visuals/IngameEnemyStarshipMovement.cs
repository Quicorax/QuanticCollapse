using UnityEngine;
using DG.Tweening;

namespace QuanticCollapse
{
    public class IngameEnemyStarshipMovement : MonoBehaviour
    {
        [SerializeField] private float floatingDispersion;

        private Vector3 intialPosition;

        void Start()
        {
            transform.DOScale(0.3f, 4f).SetEase(Ease.OutBack);
            intialPosition = transform.position;
            InitFloatation();
        }
        void InitFloatation()
        {
            float rngY = Random.Range(-floatingDispersion, floatingDispersion);
            float rngX = Random.Range(-floatingDispersion, floatingDispersion);

            transform.DOLocalRotate(Vector3.forward * (rngX > 0 ? 2f : -2f), 2f);
            transform.DOMove(intialPosition + new Vector3(rngX, rngY, 0), 2f).SetEase(Ease.InOutSine).OnComplete(() => InitFloatation());
        }
    }
}