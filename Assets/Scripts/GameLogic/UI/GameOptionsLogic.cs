using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace QuanticCollapse
{
    public class GameOptionsLogic : MonoBehaviour
    {
        [SerializeField] private GameplayCanvasManager _canvas;
        [SerializeField] private Toggle _optionsToggle;
        [SerializeField] private float _panelLateralOffset;

        private CanvasGroup _canvasGroup;

        private float _originalX;
        private float _hiddenX;

        private PopUpService _popUps;
        private LocalizationService _localization;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _popUps = ServiceLocator.GetService<PopUpService>();
            _localization = ServiceLocator.GetService<LocalizationService>();
        }
        void Start()
        {
            gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
            _originalX = transform.GetChild(0).position.x;

            _hiddenX = _originalX + _panelLateralOffset;

            transform.GetChild(0).DOMoveX(_hiddenX, 0);
        }

        public void TurnOptionsPanel(bool on)
        {
            _optionsToggle.interactable = false;

            if (on)
                gameObject.SetActive(true);

            _canvasGroup.DOFade(on ? 1 : 0, .25f);
            transform.GetChild(0).DOMoveX(on ? _originalX : _hiddenX, 0.5f).OnComplete(() =>
            {
                if (!on)
                    gameObject.SetActive(false);

                _optionsToggle.interactable = true;
            });
        }

        public void OpenExitPopUp()
        {
            _popUps.SpawnPopUp(transform.parent, new IPopUpComponentData[]
            {
            _popUps.AddHeader(_localization.Localize("GAMEPLAY_MISSION_ESCAPE_HEADER"), true),
            _popUps.AddImage("Skull", string.Empty),
            _popUps.AddText(_localization.Localize("GAMEPLAY_MISSION_ESCAPE_BODY")),
            _popUps.AddButton(_localization.Localize("GAMEPLAY_MISSION_CONFIRMESCAPE"), Retreat, true),
            _popUps.AddCloseButton(),
            });
        }

        private void Retreat() => _canvas.RetreatFromMission();

        public void ShowCredits()
        {
            _popUps.SpawnPopUp(transform.parent, new IPopUpComponentData[]
            {
            _popUps.AddHeader(_localization.Localize("LOBBY_MAIN_CREDITS_HEADER"), true),
            _popUps.AddText("<size=150%>" + _localization.Localize("LOBBY_MAIN_CREDITS_BODY") + "<b>Quicorax</b>"),
            _popUps.AddText("<align=\"left\"><indent=5%><i>Quantic Collapse</i> " + _localization.Localize("LOBBY_MAIN_CREDITS_ASSETS")),
            _popUps.AddText("<b>Kenney Assets</b>: \n" + _localization.Localize("LOBBY_MAIN_CREDITS_KENNEY")),
            _popUps.AddText("<b>Quaternius</b>: \n" + _localization.Localize("LOBBY_MAIN_CREDITS_QUATERNIUS")),
            _popUps.AddText("<b>Iconian Fonts</b>: \n" + _localization.Localize("LOBBY_MAIN_CREDITS_ICIONIAN")),
            _popUps.AddCloseButton(),
            });
        }

        public void DeleteLocalFiles()
        {
            _popUps.SpawnPopUp(transform.parent, new IPopUpComponentData[]
            {
            _popUps.AddHeader("DELETE LOCAL FILES", true),
            _popUps.AddText("Are you shure you want to delete your local files?"),
            _popUps.AddText("The following local files will be deleted:"),
            _popUps.AddText("<align=\"left\"><indent=5%><b>Game Progression</b> \n <indent=5%><b>Game Setting</b>"),
            _popUps.AddButton("Close game and delete", ConfirmDeleteFiles, true),
            _popUps.AddCloseButton(),
            });
        }

        void ConfirmDeleteFiles()
        {
            ServiceLocator.GetService<SaveLoadService>().DeleteLocalFiles();
#if !UNITY_EDITOR
        Application.Quit();
#endif
        }
    }
}