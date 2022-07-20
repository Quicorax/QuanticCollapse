using UnityEngine;

public class BoosterActionC : MonoBehaviour
{
    public void Execute(Vector2[] coords)
    {
        EventManager.Instance.BoosterSameKindCheckInteraction(coords);
    }
}
