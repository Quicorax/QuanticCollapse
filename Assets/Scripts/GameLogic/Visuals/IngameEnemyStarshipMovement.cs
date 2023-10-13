using UnityEngine;
using DG.Tweening;

namespace QuanticCollapse
{
    public class IngameEnemyStarshipMovement : MonoBehaviour
    {
        [SerializeField] private GenericEventBus _WinConditionEventBus;
        [SerializeField] private float floatingDispersion;

        private Vector3 _intialPosition;

        private void Awake()
        {
            _WinConditionEventBus.Event += StarshipDestruction;
        }

        private void OnDisable()
        {
            _WinConditionEventBus.Event -= StarshipDestruction;
        }

        private void Start()
        {
            transform.DOScale(0.3f, 4f).SetEase(Ease.OutBack);
            _intialPosition = transform.position;
            InitFloatation();
        }

        private void InitFloatation()
        {
            var rngY = Random.Range(-floatingDispersion, floatingDispersion);
            var rngX = Random.Range(-floatingDispersion, floatingDispersion);

            transform.DOLocalRotate(Vector3.forward * (rngX > 0 ? 2f : -2f), 2f);
            transform.DOMove(_intialPosition + new Vector3(rngX, rngY, 0), 2f).SetEase(Ease.InOutSine)
                .OnComplete(InitFloatation);
        }

        private void StarshipDestruction()
        {
            Destroy(gameObject);
        }
    }
}