using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    public bool blockLaserBoosterInput;

    float _cellCoordsOffset = 0.4f;

    Plane globalPlane;
    Vector2 _tappedCoords;

    [SerializeField] private TapOnCoordsEventBus _TapOnCoordsEventBus;

    void Start()
    {
        globalPlane = new Plane(Vector3.forward, Vector3.zero);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            CallCoordsCheck();
    }

    void CallCoordsCheck()
    {
        Ray globalRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (globalPlane.Raycast(globalRay, out float distance))
        {
            _tappedCoords = new Vector3(Mathf.FloorToInt(globalRay.GetPoint(distance).x + _cellCoordsOffset), Mathf.FloorToInt(globalRay.GetPoint(distance).y + _cellCoordsOffset));
            _TapOnCoordsEventBus.NotifyEvent(_tappedCoords, blockLaserBoosterInput);

            blockLaserBoosterInput = false;
        }
    }
}
