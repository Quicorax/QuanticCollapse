using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Unity.Burst.CompilerServices;

public class GameplayCanvasManager : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private GenericEventBus _AudioSettingsChanged;
    [SerializeField] private GenericEventBus _PlayerInteractionEventBus;
    [SerializeField] private GenericEventBus _TurnEndedEventBus;
    [SerializeField] private GenericEventBus _LoseConditionEventBus;
    [SerializeField] private AddScoreEventBus _AddScoreEventBus;

    [SerializeField] private Slider[] moduleSlider;
    [SerializeField] private GameObject[] turnEnergyVisuals;
    [SerializeField] private Toggle toggleSFX;
    [SerializeField] private Toggle toggleMusic;

    private MasterSceneTransitioner _masterSceneManager;

    private int interactionsRemaining;

    private AnalyticsGameService _analytics;
    private GameProgressionService _gameProgression;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
        _LoseConditionEventBus.Event += PlayerLose;
        _AddScoreEventBus.Event += AddScore;
        _PlayerInteractionEventBus.Event += Interaction;
        _TurnEndedEventBus.Event += ResetModulesCanvas;

        _analytics = ServiceLocator.GetService<AnalyticsGameService>();
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();

    }

    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
        _LoseConditionEventBus.Event -= PlayerLose;
        _AddScoreEventBus.Event -= AddScore;
        _PlayerInteractionEventBus.Event -= Interaction;
        _TurnEndedEventBus.Event -= ResetModulesCanvas;
    }

    private void Start()
    {
        interactionsRemaining = 5;
        SetModulesPowerThreshold();

        toggleSFX.isOn = _gameProgression.CheckSFXOff();
        toggleMusic.isOn = _gameProgression.CheckMusicOff();
    }
    void SetMasterReference(MasterSceneTransitioner masterReference) => _masterSceneManager = masterReference;

    void Interaction()
    {
        interactionsRemaining--;
        CallCanvasTurnUpdate(interactionsRemaining);
    }
    void AddScore(ElementKind kind, int amount) => AddScoreOfKind(kind, amount);
    void CallCanvasTurnUpdate(int i) => SetTurns(i);
    void AddScoreOfKind(ElementKind kind, int amount)
    {
        int kindIndex = (int)kind;

        AddModuleSlider(kindIndex, amount);
    }
    void SetModulesPowerThreshold()
    {
        for (int i = 0; i < 4; i++)
            SetMaxModuleSliderPower(i, 15);
    }

    void ResetModulesCanvas()
    {
        interactionsRemaining = 5;
        CallCanvasTurnUpdate(interactionsRemaining);

        for (int i = 0; i < 4; i++)
            ResetModuleSlider(i);
    }
    public void SetTurns(int turnIndex) 
    {
        for (int i = 0; i < turnEnergyVisuals.Length; i++)
        {
            if(turnIndex > i)
                turnEnergyVisuals[i].transform.DOScale(Vector2.one, 0.3f).SetEase(Ease.OutBack);
            else
                turnEnergyVisuals[i].transform.DOScale(Vector2.zero, 0.3f).SetEase(Ease.InBack);
        }
    }

    public void SetMaxModuleSliderPower(int moduleIndex, int maxPower) { moduleSlider[moduleIndex].maxValue = maxPower; }
    public void AddModuleSlider(int moduleIndex, int value) { moduleSlider[moduleIndex].value += value; }
    public void ResetModuleSlider(int moduleIndex) { moduleSlider[moduleIndex].value = 0; }
    public void RetreatFromMission() 
    {
        _masterSceneManager.ResetLevelData();
        _masterSceneManager.NavigateToInitialScene(); 
    }
    public  void ReplayMission() => _masterSceneManager.NavigateToGamePlayScene(); 


    public void PlayerWin(int[] rewards)
    {
        _analytics.SendEvent(Constants.LevelWin,
            new Dictionary<string, object>() { { Constants.LevelIndex, _masterSceneManager.LevelData.Level } });

        List<PopUpComponentData> Modules = new();
        Modules.Add(new HeaderPopUpComponentData(Constants.MissionCompleted, true));
        Modules.Add(new TextPopUpComponentData(Constants.MissionCompletedLog));

        if (rewards[0] > 0)
            Modules.Add(new ImagePopUpComponentData(Constants.Reputation, Constants.X + rewards[0]));
        if (rewards[1] > 0)
            Modules.Add(new ImagePopUpComponentData(Constants.Dilithium, Constants.X + rewards[1]));
        if (rewards[2] > 0)
            Modules.Add(new ImagePopUpComponentData(Constants.AllianceCredits, Constants.X + rewards[2]));
            
        Modules.Add(new ButtonPopUpComponentData(Constants.Continue, RetreatFromMission, true));

        Addressables.LoadAssetAsync<GameObject>(Constants.ModularPopUp).Completed += handle =>
        {
            Addressables.InstantiateAsync(Constants.ModularPopUp, transform)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };

    }
    public async void PlayerLose()
    {
        _analytics.SendEvent(Constants.LevelLose,
            new Dictionary<string, object>() { { Constants.LevelIndex, _masterSceneManager.LevelData.Level } });

        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData(Constants.MissionFailed, true),
            new TextPopUpComponentData(Constants.MissionFailedLog),

            new ImagePopUpComponentData(Constants.SkullIcon),

            new ButtonPopUpComponentData(Constants.AbandoneMission, RetreatFromMission, true),
            new ButtonPopUpComponentData(Constants.RepeatMission, ReplayMission, true),
        };

        Addressables.LoadAssetAsync<GameObject>(Constants.ModularPopUp).Completed += handle =>
        {
            Addressables.InstantiateAsync(Constants.ModularPopUp, transform)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };

    }

    public void CancellSFX(bool cancel)
    {
        _gameProgression.SetSFXOff(cancel);
        _AudioSettingsChanged.NotifyEvent();
    }

    public void CancellMusic(bool cancel)
    {
        _gameProgression.SetMusicOff(cancel);
        _AudioSettingsChanged.NotifyEvent();
    }
}
