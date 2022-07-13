using UnityEngine;

public class PlayerGridInteraction : MonoBehaviour
{
    public InputManager inputManager;

    VirtualGridManager virtualGridManager;

    void Awake()
    {
        virtualGridManager = GetComponent<VirtualGridManager>();
        inputManager.OnTapp += OnTap;

    }
    void OnDestroy()
    {
        inputManager.OnTapp -= OnTap;
    }

    void OnTap(Vector2 coords)
    {
        virtualGridManager.CheckElementOnGrid(coords);
    }
}
