using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InitialSceneGeneralCanvas : MonoBehaviour
{
    [SerializeField] private GenericEventBus _AudioSettingsChanged;

    [SerializeField] private CanvasGroup initialCanvasGroup;
    [SerializeField] private CanvasGroup persistentCanvasGroup;

    private CanvasGroup shopCanvasGroup;
    private CanvasGroup hangarCanvasGroup;

    [SerializeField] private Transform _shopView;
    [SerializeField] private Transform _hangarView;

    [SerializeField] private Transform shopIcon;
    [SerializeField] private Transform hangarIcon;

    [SerializeField] private Toggle toggleSFX;
    [SerializeField] private Toggle toggleMusic;

    private GameProgressionService _gameProgression;
    private LocalizationService _localization;
    private PopUpService _popUps;

    private float shopIconInitialY;
    private float hangarIconInitialY;

    private bool shopVisible;
    private bool hangarVisible;
    private bool onTween;

    private void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        _localization = ServiceLocator.GetService<LocalizationService>();
        _popUps = ServiceLocator.GetService<PopUpService>();
    }
    private void Start()
    {
        if(PlayerPrefs.GetInt("ConditionsAccepted") == 0)
        {
            _popUps.AddHeader(_localization.Localize("LOBBY_MAIN_PRIVACY_HEADER"), true);
            _popUps.AddText(_localization.Localize("LOBBY_MAIN_PRIVACY_BODY"));
            _popUps.AddButton(_localization.Localize("LOBBY_MAIN_PRIVACY_READ"), 
                ()=> Application.OpenURL("https://quicorax.github.io/"), false);
            _popUps.AddButton(_localization.Localize("LOBBY_MAIN_PRIVACY_ACCEPT"), 
                ()=> PlayerPrefs.SetInt("ConditionsAccepted", 1), true);
            _popUps.AddButton(_localization.Localize("LOBBY_MAIN_PRIVACY_REJECT"), 
                ()=> Application.Quit(), false);

            _popUps.SpawnPopUp(transform);
        }

        shopIconInitialY = shopIcon.position.y;
        hangarIconInitialY = hangarIcon.position.y;

        HideShopElements(true);
        HideHangarElements(true);

        toggleSFX.isOn = _gameProgression.CheckSFXOff();
        toggleMusic.isOn = _gameProgression.CheckMusicOff();
    }
    public void CanvasEngageTrigger(bool hide)
    {
        if (onTween)
            return;
        onTween = true;
        
        HideAllInitialElements(hide);
        persistentCanvasGroup.DOFade(hide ? 0 : 1, 0.5f).OnComplete(()=> onTween = false);
    }

    public void TransitionToInitialCanvas()
    {
        if (onTween)
            return;
        onTween = true;

        persistentCanvasGroup.DOFade(1, 0.5f);
        HideAllInitialElements(false);

        if(shopVisible)
            HideShopElements(true);
        if(hangarVisible)
            HideHangarElements(true);
    }

    public void TransitionToShopCanvas()
    {
        if (onTween)
            return;
        onTween = true;

        HideAllInitialElements(true);
        HideShopElements(false);
    }
    public void TransitionToHangarCanvas()
    {
        if (onTween)
            return;
        onTween = true;

        HideAllInitialElements(true);
        HideHangarElements(false);
        persistentCanvasGroup.DOFade(0, 0.5f);
    }

    void HideAllInitialElements(bool hide)
    {
        initialCanvasGroup.interactable = !hide;
        initialCanvasGroup.blocksRaycasts = !hide;

        initialCanvasGroup.DOFade(hide ? 0 : 1, 0.25f);
        shopIcon.DOMoveY(hide ? shopIconInitialY - 300 : shopIconInitialY, 0.5f);
        hangarIcon.DOMoveY(hide ? hangarIconInitialY - 300 : hangarIconInitialY, 0.5f).OnComplete(() => onTween = false);
    }
    void HideShopElements(bool hide)
    {
        shopVisible = !hide;

        shopCanvasGroup ??= _shopView.GetComponent<CanvasGroup>();

        shopCanvasGroup.interactable = !hide;
        shopCanvasGroup.DOFade(hide ? 0 : 1, 0.5f);
        _shopView.DOMoveX(hide ? -Screen.width : Screen.width, 0.5f).SetRelative().OnComplete(() => onTween = false);
    }
    void HideHangarElements(bool hide)
    {
        hangarVisible = !hide;

        hangarCanvasGroup ??= _hangarView.GetComponent<CanvasGroup>();

        hangarCanvasGroup.interactable = !hide;
        hangarCanvasGroup.DOFade(hide ? 0 : 1, 0.5f);
        _hangarView.DOMoveX(hide ? Screen.width : -Screen.width, 0.5f).SetRelative().OnComplete(() => onTween = false);
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
