using UnityEngine;

public class BaseBooster : ScriptableObject
{
    public ElementKind boosterKind;

    public virtual void OnInteraction(Vector2Int initialCoords, GridController Controller)
    {
    }
}
