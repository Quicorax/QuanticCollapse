﻿using UnityEngine;

public class BaseBooster : ScriptableObject
{
    public string boosterName;
    public GameObject boosterPrefab;

    public BoosterActionEventBus _BoosterActionEventBus;

    public virtual void OnInteraction(Vector2 initialCoords)
    {
    }
}
