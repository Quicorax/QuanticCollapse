using System.Collections;
using System.Collections.Generic;
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
    private AddressablesService _addressables;
    private PopUpService _popUps;
    private void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
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
            _addressables.SpawnAddressable<LevelView>(Constants.Level, _parent, x=> x.Initialize(levelModels, OnNavigateToLevel));

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
            
            new HeaderPopUpComponentData(Constants.EmptyResource, true),
            new ImagePopUpComponentData(Constants.Dilithium),
            new ButtonPopUpComponentData(Constants.Buy, OpenShop, true),
            new CloseButtonPopUpComponentData(),
        };

        _popUps.SpawnPopUp(Modules, transform.parent);
    }
    public void OpenReputationPopUp()
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData(Constants.EmptyResource, true),
            new ImagePopUpComponentData(Constants.Reputation),
            new CloseButtonPopUpComponentData(),
        };

        _popUps.SpawnPopUp(Modules, transform.parent);
    }
    void OpenShop() => _canvas.TransitionToShopCanvas();
}

