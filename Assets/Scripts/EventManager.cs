using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public delegate void OnInteractedWithGrid();
    public event OnInteractedWithGrid OnInteraction;

    public delegate void AddScore(ElementKind kind, int amount);
    public event AddScore OnAddScore;

    public event Action OnTurnEnded = delegate { };
    public event Action OnExternalBoosterUsed = delegate { };

    public delegate void OnScreenTapped(Vector2 coords, bool isExternalBoosterInput);
    public event OnScreenTapped OnTapp;

    public delegate void OnStarshipAction(bool player, ElementKind kind, int force);
    public event OnStarshipAction starshipActivateModule;


    public static EventManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void TurnEnded() => OnTurnEnded();
    public void ExternalBoosterUsed() => OnExternalBoosterUsed();
    public void Interaction() => OnInteraction();
    public void AddScoreBlock(ElementKind kind, int score) => OnAddScore(kind, score);
    public void Tapp(Vector2 coords, bool isExternalBoosterInput) => OnTapp(coords, isExternalBoosterInput);
    public void StarshipActivateModule(bool player, ElementKind kind, int force) => starshipActivateModule(player, kind, force);
}
