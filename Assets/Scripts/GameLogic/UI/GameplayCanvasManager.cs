using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

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

    private GameProgressionService _gameProgression;
    private LocalizationService _localization;
    private AnalyticsGameService _analytics;
    private PopUpService _popUps;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
        _LoseConditionEventBus.Event += PlayerLose;
        _AddScoreEventBus.Event += AddScore;
        _PlayerInteractionEventBus.Event += Interaction;
        _TurnEndedEventBus.Event += ResetModulesCanvas;

        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        _localization = ServiceLocator.GetService<LocalizationService>();
        _analytics = ServiceLocator.GetService<AnalyticsGameService>();
        _popUps = ServiceLocator.GetService<PopUpService>();
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
        _analytics.SendEvent("level_win",
            new Dictionary<string, object>() { { "level_index", _masterSceneManager.LevelData.Level } });

        List<IPopUpComponentData> Modules = new();
        _popUps.AddHeader(_localization.Localize("GAMEPLAY_MISSION_COMPLETED"), true);
        _popUps.AddText(_localization.Localize("GAMEPLAY_MISSION_REWARDS"));

        if (rewards[0] > 0)
            _popUps.AddImage("Reputation", "x" + rewards[0]);
        if (rewards[1] > 0)
            _popUps.AddImage("Dilithium", "x" + rewards[1]);
        if (rewards[2] > 0)
            _popUps.AddImage("AllianceCredits", "x" + rewards[2]);
            
        _popUps.AddButton(_localization.Localize("GAMEPLAY_MISSION_CONTINUE"), RetreatFromMission, true);
        _popUps.SpawnPopUp(transform);
    }
    public void PlayerLose()
    {
        _analytics.SendEvent("level_lose",
            new Dictionary<string, object>() { { "level_index", _masterSceneManager.LevelData.Level } });

        _popUps.AddHeader(_localization.Localize("GAMEPLAY_MISSION_FAILED"), true);
        _popUps.AddText(_localization.Localize("GAMEPLAY_MISSION_SHIPDISABLED"));
        _popUps.AddImage("Skull", string.Empty);
        _popUps.AddButton(_localization.Localize("GAMEPLAY_MISSION_ABANDONE"), RetreatFromMission, true);
        _popUps.AddButton(_localization.Localize("GAMEPLAY_MISSION_REPEAT"), ReplayMission, true);
        _popUps.AddCloseButton();

        _popUps.SpawnPopUp(transform);
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
