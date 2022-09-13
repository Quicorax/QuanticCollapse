using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.Analytics;

public class GameplayCanvasManager : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private GenericEventBus _PlayerInteractionEventBus;
    [SerializeField] private GenericEventBus _TurnEndedEventBus;
    [SerializeField] private GenericEventBus _LoseConditionEventBus;
    [SerializeField] private AddScoreEventBus _AddScoreEventBus;

    [SerializeField] private Slider[] moduleSlider;
    [SerializeField] private GameObject[] turnEnergyVisuals;
    [SerializeField] private Toggle toggleSFX;
    [SerializeField] private Toggle toggleMusic;

    private MasterSceneManager _masterSceneManager;
    private AudioLogic audioLogic;

    private int interactionsRemaining;

    private AnalyticsGameService _analytics;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
        _LoseConditionEventBus.Event += PlayerLose;
        _AddScoreEventBus.Event += AddScore;
        _PlayerInteractionEventBus.Event += Interaction;
        _TurnEndedEventBus.Event += ResetModulesCanvas;

        _analytics = ServiceLocator.GetService<AnalyticsGameService>();

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
    }
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
    void SetMasterReference(MasterSceneManager masterReference)
    {
        _masterSceneManager = masterReference;
        audioLogic = masterReference.AudioLogic;
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
    public void ReplayMission() { _masterSceneManager.NavigateToGamePlayScene(); }


    public void PlayerWin(int[] rewards)
    {
        List<PopUpComponentData> Modules = new();
        Modules.Add(new HeaderPopUpComponentData("Mission Complete", true));
        Modules.Add(new TextPopUpComponentData("Rewards:"));

        if (rewards[0] > 0)
            Modules.Add(new ImagePopUpComponentData("Reputation", "x" + rewards[0]));
        if (rewards[1] > 0)
            Modules.Add(new ImagePopUpComponentData("Dilithium", "x" + rewards[1]));
        if (rewards[2] > 0)
            Modules.Add(new ImagePopUpComponentData("AlianceCredits", "x" + rewards[2]));
            
        Modules.Add(new ButtonPopUpComponentData("Continue", RetreatFromMission, true));

        Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
        {
            Addressables.InstantiateAsync("Modular_PopUp", transform)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };

        _analytics.SendEvent("level_win",
            new Dictionary<string, object>() { { "level_index", _masterSceneManager.LevelData.Level } });
    }
    public void PlayerLose()
    {
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData("Mission Failed", true),
            new TextPopUpComponentData("Your ship was disabled"),

            new ImagePopUpComponentData("Skull"),

            new ButtonPopUpComponentData("Abandone Mission", RetreatFromMission, true),
            new ButtonPopUpComponentData("Repeat Mission", ReplayMission, true),
        };

        Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
        {
            Addressables.InstantiateAsync("Modular_PopUp", transform)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };

        _analytics.SendEvent("level_lose",
            new Dictionary<string, object>() { { "level_index", _masterSceneManager.LevelData.Level } });
    }

    public void CancellSFX(bool cancel)
    {
        _masterSceneManager.SaveFiles.Configuration.IsSFXOn = !cancel;
        audioLogic.CancellSFXCall(!_masterSceneManager.SaveFiles.Configuration.IsSFXOn);
    }

    public void CancellMusic(bool cancel)
    {
        _masterSceneManager.SaveFiles.Configuration.IsMusicOn = !cancel;
        audioLogic.CancellMusicCall(!_masterSceneManager.SaveFiles.Configuration.IsMusicOn);
    }
}
