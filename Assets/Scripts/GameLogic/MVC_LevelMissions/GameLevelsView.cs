using DG.Tweening;
using System.Collections;
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
    [SerializeField] private Transform _parent;

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
        SpawnPopUp popUp = new SpawnPopUp(transform);
        popUp.GeneratePopUp(Dilithium, OpenShop);
    }

    public void OpenReputationPopUp()
    {
        SpawnPopUp popUp = new SpawnPopUp(transform);
        popUp.GeneratePopUp(Reputation);
    }
    void OpenShop() => _canvas.TransitionToShopCanvas();
}

