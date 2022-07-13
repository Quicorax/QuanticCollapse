using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    int interactionsRemaining;
    public int maxInteractionsPerTurn;

    public delegate void OnInteractedWithGrid(ElementKind kind, int amount);
    public event OnInteractedWithGrid OnInteraction;

    public event Action OnTurnEnded = delegate { };

    void Start()
    {
        ResetTurn();
    }
    public void InteractionUsed(ElementKind kind, int amount)
    {
        OnInteraction(kind, amount);

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
        OnTurnEnded();
        ResetTurn();

        //Enemy Actions 
    }

}
