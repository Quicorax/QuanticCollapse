using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
            if(!on) 
                gameObject.SetActive(false);

            _optionsToggle.interactable = true;
        });
    }

    public void OpenExitPopUp()
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData(Constants.Escape, true),
            new ImagePopUpComponentData(Constants.SkullIcon),
            new TextPopUpComponentData(Constants.EscapeLog),
            new ButtonPopUpComponentData(Constants.ConfirmEscape, Retreat, true),
            new CloseButtonPopUpComponentData()
        };
        _popUps.SpawnPopUp(Modules, transform.parent);
    }

    private void Retreat() => _canvas.RetreatFromMission();

    public void ShowCredits()
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData(_localization.Localize("LOBBY_MAIN_CREDITS_HEADER"), true),
            new TextPopUpComponentData("<size=150%>" + _localization.Localize("LOBBY_MAIN_CREDITS_BODY") + "<b>Quicorax</b>"),
            new TextPopUpComponentData("<align=\"left\"><indent=5%><i>Quantic Collapse</i> " + _localization.Localize("LOBBY_MAIN_CREDITS_ASSETS")),
            new TextPopUpComponentData("<b>Kenney Assets</b>: \n" + _localization.Localize("LOBBY_MAIN_CREDITS_KENNEY")),
            new TextPopUpComponentData("<b>Quaternius</b>: \n" + _localization.Localize("LOBBY_MAIN_CREDITS_QUATERNIUS")),
            new TextPopUpComponentData("<b>Iconian Fonts</b>: \n" + _localization.Localize("LOBBY_MAIN_CREDITS_ICIONIAN")),
            new CloseButtonPopUpComponentData()
        };
        _popUps.SpawnPopUp(Modules, transform.parent);
    }

    public void DeleteLocalFiles()
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData("DELETE LOCAL FILES", true),
            new TextPopUpComponentData("Are you shure you want to delete your local files?"),
            new TextPopUpComponentData("The following local files will be deleted:"),
            new TextPopUpComponentData("<align=\"left\"><indent=5%><b>Game Progression</b> \n <indent=5%><b>Game Setting</b>"),
            new ButtonPopUpComponentData("Close game and delete", ConfirmDeleteFiles, true),
            new CloseButtonPopUpComponentData()
        };
        _popUps.SpawnPopUp(Modules, transform.parent);
    }

    void ConfirmDeleteFiles()
    {
        ServiceLocator.GetService<SaveLoadService>().DeleteLocalFiles();
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
