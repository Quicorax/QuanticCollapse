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
    private GameConfigService _gameConfig;
    private PopUpService _popUps;

    private void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        _localization = ServiceLocator.GetService<LocalizationService>();
        _addressables = ServiceLocator.GetService<AddressablesService>();
        _gameConfig = ServiceLocator.GetService<GameConfigService>();
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

        foreach (var levelModel in _gameConfig.LevelsModel)
        {
            _addressables.SpawnAddressable<LevelView>("MissionElement", _parent, x=> x.Initialize(levelModel, OnNavigateToLevel));

            _parent.sizeDelta += new Vector2(0, 120f);
        }
    }

    private void OnNavigateToLevel(LevelModel levelModel)
    {

        if (_gameProgression.CheckElement("Reputation") >= levelModel.ReputationCap)
        {
            if (_gameProgression.CheckElement("Dilithium") > 0)
            {
                StartCoroutine(DelayedTransition(levelModel));
            }
            else
                OpenEmptyResourcePopUp("Dilithium", true);
        }
        else
            OpenEmptyResourcePopUp("Reputation", false);

    }

    IEnumerator DelayedTransition(LevelModel levelModel)
    {
        yield return StartCoroutine(_cinematicTransition.CinematicTransition());
        GameLevelsController.NavigateToLevel(levelModel);
    }

    public void OpenEmptyResourcePopUp(string resourceId, bool redirectToShop)
    {
        _popUps.AddHeader(_localization.Localize("LOBBY_MAIN_NOTENOUGHT"), true);
        _popUps.AddImage(resourceId, string.Empty);
        if(redirectToShop)
            _popUps.AddButton(_localization.Localize("LOBBY_MAIN_BUY"), OpenShop, true);
        _popUps.AddCloseButton();

        _popUps.SpawnPopUp(transform.parent);
    }

    void OpenShop() => _canvas.TransitionToShopCanvas();
}

