using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    const string DefaultInput = "Fire1";

    [SerializeField] private GenericEventBus _LoseConditionEventBus;
    [SerializeField] private GenericEventBus _WinConditionEventBus;

    [SerializeField] private VirtualGridView View;

    [SerializeField] private float _cellCoordsOffset = 0.4f;

    [HideInInspector] public bool deAthomizerBoostedInput;

    private bool _inputBlockedByGridInteraction;
    private bool _generalBlockedInput;
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
        if (Input.GetButtonDown(DefaultInput) && !_generalBlockedInput)
            CheckInputCoords();
    }

    void GeneratePlane() => _globalPlane = new Plane(Vector3.forward, Vector3.zero);
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
        View.ProcessInput(_tappedCoords, deAthomizerBoostedInput);
        deAthomizerBoostedInput = false;
    }

    public void BlockInputByGridInteraction(bool blocked) =>_inputBlockedByGridInteraction = blocked;
    public void GeneralBlockInput() =>_generalBlockedInput = true;
}
