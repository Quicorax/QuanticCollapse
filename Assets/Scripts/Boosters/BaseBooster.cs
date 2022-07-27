using System.Collections.Generic;
using UnityEngine;

public class BaseBooster : ScriptableObject
{
    public ElementKind boosterKind;

    public virtual void OnInteraction(Vector2 initialCoords, VirtualGridModel Model)
    {
    }
}
