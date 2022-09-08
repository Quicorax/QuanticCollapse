using DG.Tweening;
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

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    private MasterSceneManager _MasterSceneManager;

    public GameLevelsController GameLevelsController;

    [SerializeField] private CinematicTransitionManager _cinematicTransition;
    [SerializeField] private InitialSceneGeneralCanvas _canvas;
    [SerializeField] private LevelView _levelView;
    [SerializeField] private RectTransform _parent;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDisable()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    private void Start()
    {
        Initialize();
    }
    void SetMasterReference(MasterSceneManager masterReference) => _MasterSceneManager = masterReference;

    public void Initialize()
    {
        GameLevelsController = new(_MasterSceneManager);

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

        if (_MasterSceneManager.Inventory.CheckElementAmount(Reputation) >= levelModel.ReputationCap)
        {
            if (_MasterSceneManager.Inventory.CheckElementAmount(Dilithium) > 0)
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

