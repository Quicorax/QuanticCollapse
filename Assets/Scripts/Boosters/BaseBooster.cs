using System.Collections.Generic;
using UnityEngine;

public class BaseBooster : ScriptableObject
{
    public int boosterIndex;
    public GameObject boosterPrefab;

    public BoosterActionEventBus _BoosterActionEventBus;

    public virtual void OnInteraction(Vector2 initialCoords)
    {
    }
}
