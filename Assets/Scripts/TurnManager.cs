using UnityEngine;

public class TurnManager : MonoBehaviour
{
    int interactionsRemaining;
    public int maxInteractionsPerTurn;

    private void Awake()
    {
        EventManager.Instance.OnInteraction += InteractionUsed;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnInteraction -= InteractionUsed;
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
        EventManager.Instance.TurnEnded();
        ResetTurn();
    }

}
