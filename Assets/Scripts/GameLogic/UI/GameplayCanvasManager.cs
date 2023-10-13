using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace QuanticCollapse
{
    public class GameplayCanvasManager : MonoBehaviour
    {
        [SerializeField] private SendSceneTransitionerReferenceEventBus _SceneTransitionerReference;
        [SerializeField] private AddScoreEventBus _AddScoreEventBus;
        [SerializeField] private GenericEventBus _AudioSettingsChanged;
        [SerializeField] private GenericEventBus _PlayerInteractionEventBus;
        [SerializeField] private GenericEventBus _TurnEndedEventBus;
        [SerializeField] private GenericEventBus _LoseConditionEventBus;

        [SerializeField] private Slider[] moduleSlider;
        [SerializeField] private GameObject[] turnEnergyVisuals;
        [SerializeField] private Toggle toggleSFX;
        [SerializeField] private Toggle toggleMusic;

        private SceneTransitioner _sceneTransitioner;

        private GameProgressionService _gameProgression;
        private LocalizationService _localization;
        private AnalyticsGameService _analytics;
        private PopUpService _popUps;

        private int _interactionsRemaining;

        public void RetreatFromMission()
        {
            _sceneTransitioner.ResetLevelData();
            _sceneTransitioner.NavigateToInitialScene();
        }

        public void PlayerWinPopUp(List<Reward> rewards)
        {
            _analytics.SendEvent("level_win",
                new Dictionary<string, object>() { { "level_index", _sceneTransitioner.LevelData.Level } });

            var modules = new List<IPopUpComponentData>();
            modules.Add(_popUps.AddHeader(_localization.Localize("GAMEPLAY_MISSION_COMPLETED"), true));
            modules.Add(_popUps.AddText(_localization.Localize("GAMEPLAY_MISSION_REWARDS")));

            foreach (var item in rewards)
            {
                if (item.RewardAmount > 0)
                {
                    modules.Add(_popUps.AddImage(item.RewardId, "x" + item.RewardAmount));
                }
            }

            modules.Add(_popUps.AddButton(_localization.Localize("GAMEPLAY_MISSION_CONTINUE"), RetreatFromMission, true));

            _popUps.SpawnPopUp(transform, modules.ToArray());
        }

        public void CancelSFX(bool cancel)
        {
            _gameProgression.SetSFXOff(cancel);
            _AudioSettingsChanged.NotifyEvent();
        }

        public void CancelMusic(bool cancel)
        {
            _gameProgression.SetMusicOff(cancel);
            _AudioSettingsChanged.NotifyEvent();
        }

        private void Awake()
        {
            _SceneTransitionerReference.Event += SetMasterReference;
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
            _SceneTransitionerReference.Event -= SetMasterReference;
            _LoseConditionEventBus.Event -= PlayerLosePopUp;
            _AddScoreEventBus.Event -= AddScore;
            _PlayerInteractionEventBus.Event -= Interaction;
            _TurnEndedEventBus.Event -= ResetModulesCanvas;
        }

        private void Start()
        {
            _interactionsRemaining = 5;
            SetModulesPowerThreshold();

            toggleSFX.isOn = _gameProgression.CheckSFXOff();
            toggleMusic.isOn = _gameProgression.CheckMusicOff();

            GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetEase(Ease.InCirc);
        }

        private void SetMasterReference(SceneTransitioner transitioner) => _sceneTransitioner = transitioner;

        private void Interaction()
        {
            _interactionsRemaining--;
            CallCanvasTurnUpdate(_interactionsRemaining);
        }

        private void AddScore(int kindId, int amount) => AddScoreOfKind(kindId, amount);
        private void CallCanvasTurnUpdate(int i) => SetTurns(i);
        private void AddScoreOfKind(int kindId, int amount) => AddModuleSlider(kindId, amount);

        private void SetModulesPowerThreshold()
        {
            for (int i = 0; i < 4; i++)
            {
                SetMaxModuleSliderPower(i, 15);
            }
        }

        private void ResetModulesCanvas()
        {
            _interactionsRemaining = 5;
            CallCanvasTurnUpdate(_interactionsRemaining);

            for (int i = 0; i < 4; i++)
            {
                ResetModuleSlider(i);
            }
        }

        private void SetTurns(int turnIndex)
        {
            for (int i = 0; i < turnEnergyVisuals.Length; i++)
            {
                if (turnIndex > i)
                {
                    turnEnergyVisuals[i].transform.DOScale(Vector2.one, 0.3f).SetEase(Ease.OutBack);
                }
                else
                {
                    turnEnergyVisuals[i].transform.DOScale(Vector2.zero, 0.3f).SetEase(Ease.InBack);
                }
            }
        }

        private void SetMaxModuleSliderPower(int moduleIndex, int maxPower) =>
            moduleSlider[moduleIndex].maxValue = maxPower;

        private void AddModuleSlider(int moduleIndex, int value) => moduleSlider[moduleIndex].value += value;
        private void ResetModuleSlider(int moduleIndex) => moduleSlider[moduleIndex].value = 0;
        private void ReplayMission() => _sceneTransitioner.NavigateToGamePlayScene();

        private void PlayerLosePopUp()
        {
            _analytics.SendEvent("level_lose",
                new Dictionary<string, object>() { { "level_index", _sceneTransitioner.LevelData.Level } });

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
    }
}