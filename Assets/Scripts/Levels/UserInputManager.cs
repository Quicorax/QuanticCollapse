using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    const string DefaultTapp = "Fire1";

    [SerializeField] private TapOnCoordsEventBus _TapOnCoordsEventBus;
    [SerializeField] private GenericEventBus _LoseConditionEventBus;
    [SerializeField] private GenericEventBus _WinConditionEventBus;

    [SerializeField] private float _cellCoordsOffset = 0.4f;

    [HideInInspector] public bool blockLaserBoosterInput;

    private bool _generalBlockedInput;
    private bool _inputBlockedByGridInteraction;
    private bool _buffedInput;

    private Plane _globalPlane;
    private Vector2Int _tappedCoords;

    private void Awake()
    {
        _LoseConditionEventBus.Event += GeneralBlockInput;
        _WinConditionEventBus.Event += GeneralBlockInput;
    }
    private void OnDestroy()
    {
        _LoseConditionEventBus.Event -= GeneralBlockInput;
        _WinConditionEventBus.Event -= GeneralBlockInput;
    }
    void Start()
    {
        GeneratePlane();
        _generalBlockedInput = false;
    }
    void Update()
    {
        if (Input.GetButtonDown(DefaultTapp) && !_generalBlockedInput)
            CheckInputCoords();
    }

    void GeneratePlane() { _globalPlane = new Plane(Vector3.forward, Vector3.zero); }
    void CheckInputCoords()
    {
        Ray globalRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_globalPlane.Raycast(globalRay, out float distance))
        {
            _tappedCoords = new Vector2Int(Mathf.FloorToInt(globalRay.GetPoint(distance).x + _cellCoordsOffset), Mathf.FloorToInt(globalRay.GetPoint(distance).y + _cellCoordsOffset));

            if (_tappedCoords.y < 7 && _tappedCoords.y >= 0 && _tappedCoords.x >= 0 && _tappedCoords.y < 9)
            {
                if (!_inputBlockedByGridInteraction)
                    CallValidInput();
                else if(!_buffedInput)
                {
                    _buffedInput = true;
                    Invoke(nameof(CheckInputCoords), 0.3f);
                    return;
                }
                _buffedInput = false;
            }
        }
        _tappedCoords = Vector2Int.zero;
    }

    void CallValidInput()
    {
        _TapOnCoordsEventBus.NotifyEvent(_tappedCoords, blockLaserBoosterInput);
        blockLaserBoosterInput = false;
    }

    public void BlockInputByGridInteraction(bool blocked) { _inputBlockedByGridInteraction = blocked; }
    public void GeneralBlockInput() { _generalBlockedInput = true; }
}
