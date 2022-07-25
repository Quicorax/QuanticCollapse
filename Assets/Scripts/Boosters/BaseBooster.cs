using System.Collections.Generic;
using UnityEngine;

public class BaseBooster : ScriptableObject
{
    public GameObject boosterPrefab;

    public virtual void OnInteraction(Vector2 initialCoords, VirtualGridModel Model)
    {
    }
}
