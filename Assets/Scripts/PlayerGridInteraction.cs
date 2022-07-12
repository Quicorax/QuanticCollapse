using UnityEngine;

public class PlayerGridInteraction : MonoBehaviour
{
    public InputManager inputManager;

    void Awake()
    {
        inputManager.OnTapp += OnTap;
    }
    void OnDestroy()
    {
        inputManager.OnTapp -= OnTap;
    }

    void OnTap(Vector2 coords)
    {
        Debug.Log(coords);
    }
}
