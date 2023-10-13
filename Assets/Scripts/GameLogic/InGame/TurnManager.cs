using UnityEngine;

namespace QuanticCollapse
{
    public class TurnManager : MonoBehaviour
    {
        private int interactionsRemaining;
        [SerializeField] private int maxInteractionsPerTurn;

        [SerializeField] private GenericEventBus _PlayerInteractionEventBus;
        [SerializeField] private GenericEventBus _TurnEndedEventBus;

        public void InteractionUsed()
        {
            interactionsRemaining--;

            _PlayerInteractionEventBus.NotifyEvent();

            if (interactionsRemaining <= 0)
            {
                TurnEnded();
            }
        }

        private void Start()
        {
            ResetTurn();
        }

        private void ResetTurn() => interactionsRemaining = maxInteractionsPerTurn;

        private void TurnEnded()
        {
            _TurnEndedEventBus.NotifyEvent();
            ResetTurn();
        }
    }
}