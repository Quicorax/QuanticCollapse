using UnityEngine;

public class InputManager : MonoBehaviour
{
    const string LeftMouseButton = "Fire1";

    public VirtualGridManager virtualGridManager;

    public Plane globalPlane;

    void Start()
    {
        globalPlane = new Plane(Vector3.forward, Vector3.zero);
    }

    void Update()
    {
        if (Input.GetButtonDown(LeftMouseButton))
            TapDownInput();
    }

    void TapDownInput() { CallCoordsCheck(); }

    void CallCoordsCheck()
    {
        Ray globalRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (globalPlane.Raycast(globalRay, out float distance))
        {
            Vector2 worldCoords = new Vector3(Mathf.FloorToInt(globalRay.GetPoint(distance).x + .4f), Mathf.FloorToInt(globalRay.GetPoint(distance).y + .4f));
            virtualGridManager.CheckElementOnGrid(worldCoords);
        }
    }
}
