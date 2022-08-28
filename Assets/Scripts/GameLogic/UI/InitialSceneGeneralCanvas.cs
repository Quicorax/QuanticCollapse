using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InitialSceneGeneralCanvas : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;

    private MasterSceneManager _MasterSceneManager;

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

    float shopIconInitialY;
    float hangarIconInitialY;

    bool shopVisible;
    bool hangarVisible;

    bool onTween;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    private void Start()
    {
        shopIconInitialY = shopIcon.position.y;
        hangarIconInitialY = hangarIcon.position.y;

        HideShopElements(true);
        HideHangarElements(true);

        toggleSFX.isOn = !_MasterSceneManager.SaveFiles.configuration.isSFXOn;
        toggleMusic.isOn = !_MasterSceneManager.SaveFiles.configuration.isMusicOn;
    }

    void SetMasterReference(MasterSceneManager masterReference) => _MasterSceneManager = masterReference; 
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
        _MasterSceneManager.SaveFiles.configuration.isSFXOn = !cancel;
        _MasterSceneManager.AudioLogic.CancellSFXCall(!_MasterSceneManager.SaveFiles.configuration.isSFXOn);
    }

    public void CancellMusic(bool cancel)
    {
        _MasterSceneManager.SaveFiles.configuration.isMusicOn = !cancel;
        _MasterSceneManager.AudioLogic.CancellMusicCall(!_MasterSceneManager.SaveFiles.configuration.isMusicOn);
    }
}
