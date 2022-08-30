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
    [SerializeField] private LevelView _levelView;
    [SerializeField] private Transform _parent;

    [SerializeField] private CanvasGroup DilithiumCapPopUp;
    [SerializeField] private CanvasGroup ReputationCapPopUp;

    private bool dilithiumPopUpFading;
    private bool reputationPopUpFading;

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
        if (dilithiumPopUpFading)
            return;

        dilithiumPopUpFading = true;

        DilithiumCapPopUp.alpha = 1;
        DilithiumCapPopUp.gameObject.SetActive(true);
        DilithiumCapPopUp.transform.DOPunchScale(Vector3.one * 0.1f, .5f);
        DilithiumCapPopUp.DOFade(0, 2f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            DilithiumCapPopUp.gameObject.SetActive(false);
            dilithiumPopUpFading = false;
        });
    }
    public void OpenReputationPopUp()
    {
        if (reputationPopUpFading)
            return;

        reputationPopUpFading = true;
        ReputationCapPopUp.alpha = 1;
        ReputationCapPopUp.gameObject.SetActive(true);
        ReputationCapPopUp.transform.DOPunchScale(Vector3.one * 0.1f, .5f);
        ReputationCapPopUp.DOFade(0, 2f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            ReputationCapPopUp.gameObject.SetActive(false);
            reputationPopUpFading = false;
        });
    }
}

