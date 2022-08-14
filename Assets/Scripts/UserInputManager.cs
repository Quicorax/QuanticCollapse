using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    [HideInInspector] public bool blockLaserBoosterInput;

    bool inputBlockedByGridInteraction;

    bool buffedInput;

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
            CheckInputCoords();
    }

    void CheckInputCoords()
    {
        Ray globalRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (globalPlane.Raycast(globalRay, out float distance))
        {
            _tappedCoords = new Vector3(Mathf.FloorToInt(globalRay.GetPoint(distance).x + _cellCoordsOffset), Mathf.FloorToInt(globalRay.GetPoint(distance).y + _cellCoordsOffset));

            if (_tappedCoords.y < 7 && _tappedCoords.y >= 0 && _tappedCoords.x >= 0 && _tappedCoords.y < 9)
            {
                if (!inputBlockedByGridInteraction)
                {
                    CallValidInput();
                }
                else if(!buffedInput)
                {
                    buffedInput = true;
                    Invoke(nameof(CheckInputCoords), 0.3f);
                    return;
                }

                buffedInput = false;
            }
        }
        _tappedCoords = Vector2.zero;
    }

    void CallValidInput()
    {
        _TapOnCoordsEventBus.NotifyEvent(_tappedCoords, blockLaserBoosterInput);
        blockLaserBoosterInput = false;
    }

    public void BlockInputByGridInteraction(bool blocked)
    {
        inputBlockedByGridInteraction = blocked;
    }
}
