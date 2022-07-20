using UnityEngine;

public class BaseBooster : ScriptableObject
{
    public VirtualGridManager virtualGridManager; // TODO : Make this indepenbdent from virtualManager

    public string boosterName;

    public GameObject boosterPrefab;

    public virtual void OnInteraction(Vector2 initialCoords)
    {
    }
}
