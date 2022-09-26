using System.Collections;
using UnityEngine;

public class GameLevelsView : MonoBehaviour
{
    public GameLevelsController GameLevelsController;

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;

    [SerializeField] private CinematicTransitionManager _cinematicTransition;
    [SerializeField] private InitialSceneGeneralCanvas _canvas;
    [SerializeField] private LevelView _levelView;
    [SerializeField] private RectTransform _parent;

    private MasterSceneTransitioner _sceneTransitioner;

    private GameProgressionService _gameProgression;
    private LocalizationService _localization;
    private AddressablesService _addressables;
    private PopUpService _popUps;

    private void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        _localization = ServiceLocator.GetService<LocalizationService>();
        _addressables = ServiceLocator.GetService<AddressablesService>();
        _popUps = ServiceLocator.GetService<PopUpService>();

        _MasterReference.Event += SetMasterSceneTransitionReference;
    }
    private void OnDisable()
    {
        _MasterReference.Event -= SetMasterSceneTransitionReference;
    }
    private void Start()
    {
        Initialize();
    }

    void SetMasterSceneTransitionReference(MasterSceneTransitioner sceneTransitioner) => _sceneTransitioner = sceneTransitioner;
    public void Initialize()
    {
        GameLevelsController = new(_gameProgression, _sceneTransitioner);

        foreach (var levelModels in GameLevelsController.GameLevelsModel.Levels)
        {
            _addressables.SpawnAddressable<LevelView>("LevelMissionElement_ViewObject", _parent, x=> x.Initialize(levelModels, OnNavigateToLevel));

            _parent.sizeDelta += new Vector2(0, 120f);
        }
    }

    private void OnNavigateToLevel(LevelModel levelModel)
    {

        if (_gameProgression.CheckElement(ResourcesType.Reputation) >= levelModel.ReputationCap)
        {
            if (_gameProgression.CheckElement(ResourcesType.Dilithium) > 0)
            {
                StartCoroutine(DelayedTransition(levelModel));
            }
            else
                OpenDilithiumPopUp();
        }
        else
            OpenReputationPopUp();

    }

    IEnumerator DelayedTransition(LevelModel levelModel)
    {
        yield return StartCoroutine(_cinematicTransition.CinematicTransition());
        GameLevelsController.NavigateToLevel(levelModel);
    }

    public void OpenDilithiumPopUp()
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {

            new HeaderPopUpComponentData(_localization.Localize("LOBBY_MAIN_NOTENOUGHT"), true),
            new ImagePopUpComponentData("Dilithium"),
            new ButtonPopUpComponentData(_localization.Localize("LOBBY_MAIN_BUY"), OpenShop, true),
            new CloseButtonPopUpComponentData(),
        };

        _popUps.SpawnPopUp(Modules, transform.parent);
    }
    public void OpenReputationPopUp()
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData(_localization.Localize("LOBBY_MAIN_NOTENOUGHT"), true),
            new ImagePopUpComponentData("Reputation"),
            new CloseButtonPopUpComponentData(),
        };

        _popUps.SpawnPopUp(Modules, transform.parent);
    }
    void OpenShop() => _canvas.TransitionToShopCanvas();
}

