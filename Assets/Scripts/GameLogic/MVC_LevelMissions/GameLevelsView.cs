using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Burst.CompilerServices;
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

    private void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        _addressables = ServiceLocator.GetService<AddressablesService>();

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
    public async void Initialize()
    {
        GameLevelsController = new(_gameProgression, _sceneTransitioner);

        foreach (var levelModels in GameLevelsController.GameLevelsModel.Levels)
        {
            var adrsInstance = await _addressables
                .SpawnAddressable<LevelView>(Constants.Level, _parent);

            adrsInstance.Initialize(levelModels, OnNavigateToLevel);

            _parent.sizeDelta += new Vector2(0, 120f);
        }
    }

    private void OnNavigateToLevel(LevelModel levelModel)
    {

        if (_gameProgression.CheckElement(Constants.Reputation) >= levelModel.ReputationCap)
        {
            if (_gameProgression.CheckElement(Constants.Dilithium) > 0)
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

    public async void OpenDilithiumPopUp()
    {
        List<PopUpComponentData> Modules = new()
        {
            
            new HeaderPopUpComponentData(Constants.EmptyResource, true),
            new ImagePopUpComponentData(Constants.Dilithium),
            new ButtonPopUpComponentData(Constants.Buy, OpenShop, true),
            new CloseButtonPopUpComponentData(),
        };

        var adrsInstance = await _addressables
            .SpawnAddressable<ModularPopUp>(Constants.ModularPopUp, transform.parent);

        adrsInstance.GeneratePopUp(Modules);
    }
    public async void OpenReputationPopUp()
    {
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData(Constants.EmptyResource, true),
            new ImagePopUpComponentData(Constants.Reputation),
            new CloseButtonPopUpComponentData(),
        };

        var adrsInstance = await _addressables
            .SpawnAddressable<ModularPopUp>(Constants.ModularPopUp, transform.parent);

        adrsInstance.GeneratePopUp(Modules);
    }
    void OpenShop() => _canvas.TransitionToShopCanvas();
}

