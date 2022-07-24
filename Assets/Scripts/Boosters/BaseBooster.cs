using System.Collections.Generic;
using UnityEngine;

public class BaseBooster : ScriptableObject
{
    public string boosterName;
    public GameObject boosterPrefab;

    public BoosterActionEventBus _BoosterActionEventBus;

    public virtual void OnInteraction(Vector2 initialCoords)
    {
    }
    public virtual List<Vector2> OnReturnCellsByInteraction(Vector2 initialCoords)
    {
        List<Vector2> affectedCoords = new();

        return affectedCoords;
    }
}
