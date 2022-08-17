using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    [SerializeField] private TapOnCoordsEventBus _TapOnCoordsEventBus;

    [SerializeField] private float _cellCoordsOffset = 0.4f;

    [HideInInspector] public bool blockLaserBoosterInput;

    private bool _inputBlockedByGridInteraction;
    private bool _buffedInput;

    private Plane _globalPlane;
    private Vector2 _tappedCoords;

    void Start()
    {
        GeneratePlane();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            CheckInputCoords();
    }

    void GeneratePlane() { _globalPlane = new Plane(Vector3.forward, Vector3.zero); }
    void CheckInputCoords()
    {
        Ray globalRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_globalPlane.Raycast(globalRay, out float distance))
        {
            _tappedCoords = new Vector3(Mathf.FloorToInt(globalRay.GetPoint(distance).x + _cellCoordsOffset), Mathf.FloorToInt(globalRay.GetPoint(distance).y + _cellCoordsOffset));

            if (_tappedCoords.y < 7 && _tappedCoords.y >= 0 && _tappedCoords.x >= 0 && _tappedCoords.y < 9)
            {
                if (!_inputBlockedByGridInteraction)
                {
                    CallValidInput();
                }
                else if(!_buffedInput)
                {
                    _buffedInput = true;
                    Invoke(nameof(CheckInputCoords), 0.3f);
                    return;
                }
                _buffedInput = false;
            }
        }
        _tappedCoords = Vector2.zero;
    }

    void CallValidInput()
    {
        _TapOnCoordsEventBus.NotifyEvent(_tappedCoords, blockLaserBoosterInput);
        blockLaserBoosterInput = false;
    }

    public void BlockInputByGridInteraction(bool blocked) { _inputBlockedByGridInteraction = blocked; }
}
