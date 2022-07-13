using UnityEngine;

public class InputManager : MonoBehaviour
{
    Plane globalPlane;

    Vector2 tappedCoords;

    public delegate void OnScreenTapped(Vector2 coords);
    public event OnScreenTapped OnTapp;

    void Start()
    {
        globalPlane = new Plane(Vector3.forward, Vector3.zero);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            TapDownInput();
    }

    void TapDownInput() { CallCoordsCheck(); }

    void CallCoordsCheck()
    {
        Ray globalRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (globalPlane.Raycast(globalRay, out float distance))
        {
            tappedCoords = new Vector3(Mathf.FloorToInt(globalRay.GetPoint(distance).x + .4f), Mathf.FloorToInt(globalRay.GetPoint(distance).y + .4f));

            OnTapp(tappedCoords);
        }
    }
}
