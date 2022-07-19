using UnityEngine;

public class BaseBooster : ScriptableObject
{
    public VirtualGridManager virtualGridManager;

    public string boosterName;

    public GameObject boosterPrefab;

    public virtual void OnInteraction(Vector2 initialCoords)
    {
    }
}
