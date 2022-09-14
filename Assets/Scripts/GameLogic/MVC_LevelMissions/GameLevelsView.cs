using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameLevelsView : MonoBehaviour
{
    const string Reputation = "Reputation";
    const string Dilithium = "Dilithium";
    const string LevelViewName = "LevelView_";
    const string LevelAdrsKey = "LevelMissionElement_ViewObject";

    public GameLevelsController GameLevelsController;

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;

    [SerializeField] private CinematicTransitionManager _cinematicTransition;
    [SerializeField] private InitialSceneGeneralCanvas _canvas;
    [SerializeField] private LevelView _levelView;
    [SerializeField] private RectTransform _parent;

    private MasterSceneTransitioner _sceneTransitioner;
    private GameProgressionService _gameProgression;
    private void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
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
            Addressables.LoadAssetAsync<GameObject>(LevelAdrsKey).Completed += handle =>
            {
                GameObject element = Addressables.InstantiateAsync(LevelAdrsKey, _parent).Result;
                element.GetComponent<LevelView>().Initialize(levelModels, OnNavigateToLevel);
                element.name = LevelViewName + levelModels.Level;

                _parent.sizeDelta += new Vector2(0, 120f);
            };
        }
    }

    private void OnNavigateToLevel(LevelModel levelModel)
    {

        if (_gameProgression.CheckElement(Reputation) >= levelModel.ReputationCap)
        {
            if (_gameProgression.CheckElement(Dilithium) > 0)
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
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData("You don't have enought:", true),
            new ImagePopUpComponentData(Dilithium),
            new ButtonPopUpComponentData("Buy More", OpenShop, true),
            new CloseButtonPopUpComponentData(),
        };

        Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
        {
            Addressables.InstantiateAsync("Modular_PopUp", transform.parent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }
    public void OpenReputationPopUp()
    {
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData("You don't have enought:", true),
            new ImagePopUpComponentData(Reputation),
            new CloseButtonPopUpComponentData(),
        };

        Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
        {
            Addressables.InstantiateAsync("Modular_PopUp", transform.parent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }
    void OpenShop() => _canvas.TransitionToShopCanvas();
}

