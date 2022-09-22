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
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _popUps = ServiceLocator.GetService<PopUpService>();
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
            new HeaderPopUpComponentData(Constants.Credits, true),
            new TextPopUpComponentData(Constants.CreditsSelf),
            new TextPopUpComponentData(Constants.CreditsLog),
            new TextPopUpComponentData(Constants.Kenney),
            new TextPopUpComponentData(Constants.Quaternius),
            new TextPopUpComponentData(Constants.Iconian),
            new CloseButtonPopUpComponentData()
        };
        _popUps.SpawnPopUp(Modules, transform.parent);
    }

    public void DeleteLocalFiles()
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData(Constants.DeleteFiles, true),
            new TextPopUpComponentData(Constants.DeleteFilesLog),
            new TextPopUpComponentData(Constants.DeleteFilesDetail),
            new TextPopUpComponentData(Constants.DeleteFilesSpecific),
            new ButtonPopUpComponentData(Constants.DeleteFiles, ConfirmDeleteFiles, true),
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
