using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnInteractedWithGrid();
    public event OnInteractedWithGrid OnInteraction;

    public delegate void AddScore(ElementKind kind, int amount);
    public event AddScore OnAddScore;

    public delegate void OnScreenTapped(Vector2 coords, bool isExternalBoosterInput);
    public event OnScreenTapped OnTapp;

    public delegate void OnStarshipAction(bool player, ElementKind kind, int force);
    public event OnStarshipAction starshipActivateModule;

    public delegate void OnBoosterInteraction(Vector2[] coords);
    public event OnBoosterInteraction OnBooster;

    public delegate void OnSameKindBoosterInteraction(Vector2[] coords);
    public event OnSameKindBoosterInteraction OnSameKindBooster;


    public event Action OnTurnEnded = delegate { };
    public event Action OnExternalBoosterUsed = delegate { };
    public event Action OnImposibleGrid = delegate { };

    public static EventManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void TurnEnded() => OnTurnEnded();
    public void BoosterInteraction(Vector2[] coords) => OnBooster(coords);
    public void BoosterSameKindCheckInteraction(Vector2[] coords) => OnSameKindBooster(coords);
    public void ImposibleGrid() => OnImposibleGrid();
    public void ExternalBoosterUsed() => OnExternalBoosterUsed();
    public void Interaction() => OnInteraction();
    public void AddScoreBlock(ElementKind kind, int score) => OnAddScore(kind, score);
    public void Tapp(Vector2 coords, bool isExternalBoosterInput) => OnTapp(coords, isExternalBoosterInput);
    public void StarshipActivateModule(bool player, ElementKind kind, int force) => starshipActivateModule(player, kind, force);
}
