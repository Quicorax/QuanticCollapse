using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private int interactionsRemaining;
    public int maxInteractionsPerTurn;

    [SerializeField] private GenericEventBus _PlayerInteractionEventBus;
    [SerializeField] private GenericEventBus _TurnEndedEventBus;

    void Start()
    {
        ResetTurn();
    }
    public void InteractionUsed()
    {
        interactionsRemaining--;

        _PlayerInteractionEventBus.NotifyEvent();

        if (interactionsRemaining <= 0)
            TurnEnded();
    }
    void ResetTurn()
    {
        interactionsRemaining = maxInteractionsPerTurn;
    }

    void TurnEnded()
    {
        _TurnEndedEventBus.NotifyEvent();
        ResetTurn();
    }
}
