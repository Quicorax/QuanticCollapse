using System.Collections;
using UnityEngine;

namespace QuanticCollapse
{
    public class GameLevelsView : MonoBehaviour
    {
        public GameLevelsController GameLevelsController;

        [SerializeField] 
        private SendSceneTransitionerReferenceEventBus _SceneTransitionerReference;

        [SerializeField] 
        private CinematicTransitionManager _cinematicTransition;
        [SerializeField] 
        private InitialSceneGeneralCanvas _canvas;
        [SerializeField] 
        private LevelView _levelView;
        [SerializeField] 
        private RectTransform _levelsParent;

        private SceneTransitioner _sceneTransitioner;

        private GameProgressionService _gameProgression;
        private LocalizationService _localization;
        private AddressablesService _addressables;
        private GameConfigService _gameConfig;
        private PopUpService _popUps;

        private void Awake()
        {
            _gameProgression = ServiceLocator.GetService<GameProgressionService>();
            _localization = ServiceLocator.GetService<LocalizationService>();
            _addressables = ServiceLocator.GetService<AddressablesService>();
            _gameConfig = ServiceLocator.GetService<GameConfigService>();
            _popUps = ServiceLocator.GetService<PopUpService>();

            _SceneTransitionerReference.Event += SetMasterSceneTransitionReference;
        }
        private void OnDisable()
        {
            _SceneTransitionerReference.Event -= SetMasterSceneTransitionReference;
        }
        private void Start()
        {
            Initialize();
        }

        void SetMasterSceneTransitionReference(SceneTransitioner sceneTransitioner) => _sceneTransitioner = sceneTransitioner;
        
        public void Initialize()
        {
            GameLevelsController = new(_gameProgression, _sceneTransitioner);

            foreach (LevelModel levelModel in _gameConfig.LevelsModel)
            {
                _addressables.LoadAdrsOfComponent<LevelView>("MissionElement", _levelsParent, level => 
                level.Initialize(levelModel, OnNavigateToLevel));

                _levelsParent.sizeDelta += new Vector2(0, 120f);
            }
        }

        private void OnNavigateToLevel(LevelModel levelModel)
        {
            if (_gameProgression.CheckElement("Reputation") >= levelModel.ReputationCap)
            {
                if (_gameProgression.CheckElement("Dilithium") > 0)
                    StartCoroutine(DelayedTransition(levelModel));
                else
                    OpenEmptyDilithiumPopUp();
            }
            else
                OpenEmptyReputationPopUp();
        }

        IEnumerator DelayedTransition(LevelModel levelModel)
        {
            yield return StartCoroutine(_cinematicTransition.CinematicTransition());
            GameLevelsController.NavigateToLevel(levelModel);
        }

        #region PopUps
        public void OpenEmptyDilithiumPopUp()
        {
            _popUps.SpawnPopUp(transform.parent, new IPopUpComponentData[]
            {
                _popUps.AddHeader(_localization.Localize("LOBBY_MAIN_NOTENOUGHT"), true),
                _popUps.AddImage("Dilithium", string.Empty),
                _popUps.AddButton(_localization.Localize("LOBBY_MAIN_BUY"),
                    ()=> { _canvas.TransitionToShopCanvas(); }, true),
                _popUps.AddCloseButton(),
            });
        }
        public void OpenEmptyReputationPopUp()
        {
            _popUps.SpawnPopUp(transform.parent, new IPopUpComponentData[]
            {
                _popUps.AddHeader(_localization.Localize("LOBBY_MAIN_NOTENOUGHT"), true),
                _popUps.AddImage("Reputation", string.Empty),
                _popUps.AddCloseButton(),
            });
        }
        #endregion
    }
}