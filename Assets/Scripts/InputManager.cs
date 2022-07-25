using UnityEngine;

public class InputManager : MonoBehaviour
{
    Plane globalPlane;
    Vector2 tappedCoords;

    [SerializeField] private TapOnCoordsEventBus _TapOnCoordsEventBus;

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

            _TapOnCoordsEventBus.NotifyEvent(tappedCoords);
        }
    }
}
