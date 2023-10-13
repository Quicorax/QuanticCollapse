using UnityEngine;

namespace QuanticCollapse
{
    public class UserInputManager : MonoBehaviour
    {
        [HideInInspector] public bool DeAthomizerBoostedInput;

        [SerializeField] private GenericEventBus _LoseConditionEventBus;
        [SerializeField] private GenericEventBus _WinConditionEventBus;

        [SerializeField] private GridView _view;

        [SerializeField] private float _cellCoordsOffset = 0.4f;

        private bool _inputBlockedByGridInteraction;
        private bool _generalBlockedInput;
        private bool _buffedInput;

        private Plane _globalPlane;
        private Vector2Int _tappedCoords;

        public void BlockInputByGridInteraction(bool blocked) => _inputBlockedByGridInteraction = blocked;

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

        private void Start()
        {
            GeneratePlane();
            _generalBlockedInput = false;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1") && !_generalBlockedInput)
            {
                CheckInputCoords();
            }
        }

        private void GeneratePlane() => _globalPlane = new Plane(Vector3.forward, Vector3.zero);

        private void CheckInputCoords()
        {
            var globalRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_globalPlane.Raycast(globalRay, out var distance))
            {
                _tappedCoords = new(Mathf.FloorToInt(globalRay.GetPoint(distance).x + _cellCoordsOffset),
                    Mathf.FloorToInt(globalRay.GetPoint(distance).y + _cellCoordsOffset));

                if (_tappedCoords.y < 7 && _tappedCoords.y >= 0 && _tappedCoords.x >= 0 && _tappedCoords.y < 9)
                {
                    if (!_inputBlockedByGridInteraction)
                    {
                        CallValidInput();
                    }
                    else if (!_buffedInput)
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

        private void CallValidInput()
        {
            _view.ProcessInput(_tappedCoords, DeAthomizerBoostedInput);
            DeAthomizerBoostedInput = false;
        }

        private void GeneralBlockInput() => _generalBlockedInput = true;
    }
}