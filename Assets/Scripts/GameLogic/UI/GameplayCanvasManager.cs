using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace QuanticCollapse
{
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
            _LoseConditionEventBus.Event += PlayerLosePopUp;
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
            _LoseConditionEventBus.Event -= PlayerLosePopUp;
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
        void AddScore(int kindId, int amount) => AddScoreOfKind(kindId, amount);
        void CallCanvasTurnUpdate(int i) => SetTurns(i);
        void AddScoreOfKind(int kindId, int amount) => AddModuleSlider(kindId, amount);
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
                if (turnIndex > i)
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
        public void ReplayMission() => _masterSceneManager.NavigateToGamePlayScene();


        public void PlayerWinPopUp(List<Reward> rewards)
        {
            _analytics.SendEvent("level_win",
                new Dictionary<string, object>() { { "level_index", _masterSceneManager.LevelData.Level } });

            List<IPopUpComponentData> Modules = new();
            Modules.Add(_popUps.AddHeader(_localization.Localize("GAMEPLAY_MISSION_COMPLETED"), true));
            Modules.Add(_popUps.AddText(_localization.Localize("GAMEPLAY_MISSION_REWARDS")));
            foreach (var item in rewards)
            {
                if (item.RewardAmount > 0)
                    Modules.Add(_popUps.AddImage(item.RewardId, "x" + item.RewardAmount));
            }
            Modules.Add(_popUps.AddButton(_localization.Localize("GAMEPLAY_MISSION_CONTINUE"), RetreatFromMission, true));

            _popUps.SpawnPopUp(transform, Modules.ToArray());
        }
        public void PlayerLosePopUp()
        {
            _analytics.SendEvent("level_lose",
                new Dictionary<string, object>() { { "level_index", _masterSceneManager.LevelData.Level } });

            _popUps.SpawnPopUp(transform, new IPopUpComponentData[]
            {
            _popUps.AddHeader(_localization.Localize("GAMEPLAY_MISSION_FAILED"), true),
            _popUps.AddText(_localization.Localize("GAMEPLAY_MISSION_SHIPDISABLED")),
            _popUps.AddImage("Skull", string.Empty),
            _popUps.AddButton(_localization.Localize("GAMEPLAY_MISSION_ABANDONE"), RetreatFromMission, true),
            _popUps.AddButton(_localization.Localize("GAMEPLAY_MISSION_REPEAT"), ReplayMission, true),
            _popUps.AddCloseButton(),
            });
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
}