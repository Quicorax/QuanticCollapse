using UnityEngine;

public class TurnManager : MonoBehaviour
{
    int interactionsRemaining;
    public int maxInteractionsPerTurn;

    [SerializeField]
    private GenericEventBus _PlayerInteractionEventBus;
    [SerializeField]
    private GenericEventBus _TurnEndedEventBus;

    private void Awake()
    {
        _PlayerInteractionEventBus.Event += InteractionUsed;
    }

    private void OnDestroy()
    {
        _PlayerInteractionEventBus.Event -= InteractionUsed;
    }


    void Start()
    {
        ResetTurn();
    }
    public void InteractionUsed()
    {
        interactionsRemaining--;

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
