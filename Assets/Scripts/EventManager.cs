using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public delegate void OnInteractedWithGrid(ElementKind kind, int amount);
    public event OnInteractedWithGrid OnInteraction;

    public event Action OnTurnEnded = delegate { };

    public delegate void OnScreenTapped(Vector2 coords);
    public event OnScreenTapped OnTapp;

    public delegate void OnStarshipAction(bool player, ElementKind kind, int force);
    public event OnStarshipAction starshipActivateModule;


    public static EventManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void TurnEnded() { OnTurnEnded(); }
    public void Interaction(ElementKind kind, int score) { OnInteraction(kind, score); }
    public void Tapp(Vector2 coords) { OnTapp(coords); }
    public void StarshipActivateModule(bool player, ElementKind kind, int force) { starshipActivateModule(player, kind, force); }
}
