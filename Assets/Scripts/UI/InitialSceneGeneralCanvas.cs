using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class InitialSceneGeneralCanvas : MonoBehaviour
{
    private MasterSceneManager _MasterSceneManager;
    private AudioLogic audioLogic;

    [SerializeField] private ShopManager shopManager;

    [SerializeField] private CanvasGroup initialCanvasGroup;
    [SerializeField] private CanvasGroup shopCanvasGroup;
    [SerializeField] private CanvasGroup persistentCanvasGroup;

    [SerializeField] private Transform shopCanvas;

    [SerializeField] private Transform missionLog;
    [SerializeField] private Transform shopIcon;

    float shopIconInitialY;

    [SerializeField] private TMP_Text dilithium_Text;
    [SerializeField] private TMP_Text alianceCredits_Text;
    [SerializeField] private TMP_Text reputation_Text;

    [SerializeField] private CanvasGroup DilithiumCapPopUp;
    [SerializeField] private CanvasGroup ReputationCapPopUp;

    [SerializeField] private Toggle toggleSFX;
    [SerializeField] private Toggle toggleMusic;

    bool dilithiumPopUpFading;
    bool reputationPopUpFading;
    private void Awake()
    {
        _MasterSceneManager = FindObjectOfType<MasterSceneManager>();
        audioLogic = _MasterSceneManager.AudioLogic;
    }
    private void Start()
    {
        shopIconInitialY = shopIcon.position.y;

        HideShopElemennts(true);

        toggleSFX.isOn = !_MasterSceneManager.runtimeSaveFiles.configuration.isSFXOn;
        toggleMusic.isOn = !_MasterSceneManager.runtimeSaveFiles.configuration.isMusicOn;
    }
    public void CanvasEngageTrigger(bool hide)
    {
        HideAllInitialElements(hide);
        persistentCanvasGroup.DOFade(hide ? 0 : 1, 0.5f);
    }

    public void SetDilithiumAmount(int amount) { dilithium_Text.text = amount.ToString(); }
    public void SetCreditsAmount(int amount) { alianceCredits_Text.text = amount.ToString(); }
    public void SetReputationAmount(int amount) { reputation_Text.text = amount.ToString(); }

    void HideAllInitialElements(bool hide)
    {
        initialCanvasGroup.DOFade(hide ? 0 : 1, 0.5f);

        missionLog.DOMoveX(hide ? Screen.width : -Screen.width, 0.5f).SetRelative();

        shopIcon.DOMoveY(hide ? shopIconInitialY - 300 : shopIconInitialY, 0.5f);
    }
    void HideShopElemennts(bool hide)
    {
        shopCanvasGroup.DOFade(hide ? 0 : 1, 0.5f);
        shopCanvas.DOMoveX(hide ? -Screen.width : Screen.width, 0.5f).SetRelative();
    }
    public void TransitionToShopCanvas()
    {
        HideAllInitialElements(true);
        HideShopElemennts(false);
    }
    public void TransitionToInitialCanvas()
    {
        HideAllInitialElements(false);
        HideShopElemennts(true);
    }
    public void OpenDilithiumPopUp() 
    {
        if (dilithiumPopUpFading)
            return;

        dilithiumPopUpFading = true;

        DilithiumCapPopUp.alpha = 1;
        DilithiumCapPopUp.gameObject.SetActive(true);
        DilithiumCapPopUp.transform.DOPunchScale(Vector3.one * 0.1f, .5f);
        DilithiumCapPopUp.DOFade(0, 2f).SetEase(Ease.InCirc).OnComplete(() => 
        { 
            DilithiumCapPopUp.gameObject.SetActive(false);
            dilithiumPopUpFading = false;
        });
    }
    public void OpenReputationPopUp() 
    {
        if (reputationPopUpFading)
            return;

        reputationPopUpFading = true;
        ReputationCapPopUp.alpha = 1;
        ReputationCapPopUp.gameObject.SetActive(true);
        ReputationCapPopUp.transform.DOPunchScale(Vector3.one * 0.1f, .5f);
        ReputationCapPopUp.DOFade(0, 2f).SetEase(Ease.InCirc).OnComplete(() => 
        { 
            ReputationCapPopUp.gameObject.SetActive(false);
            reputationPopUpFading = false;
        });
    }
    public void AskBuyExternalBooster(int externalBoosterKindIndex)
    {
        shopManager.TryBuyExternalBooster((ExternalBoosterKind)externalBoosterKindIndex, out int remainingCredits);
        SetCreditsAmount(remainingCredits);
    }

    public void AskBuyDilithium()
    {
        shopManager.TryBuyDilithium(out int remainingCredits);
        SetCreditsAmount(remainingCredits);
    }

    public void AskBuyAlianceCredits() { shopManager.TryBuyCredits(); }

    public void CancellSFX(bool cancel)
    {
        _MasterSceneManager.runtimeSaveFiles.configuration.isSFXOn = !cancel;
        audioLogic.CancellSFXCall(!_MasterSceneManager.runtimeSaveFiles.configuration.isSFXOn);
    }

    public void CancellMusic(bool cancel)
    {
        _MasterSceneManager.runtimeSaveFiles.configuration.isMusicOn = !cancel;
        audioLogic.CancellMusicCall(!_MasterSceneManager.runtimeSaveFiles.configuration.isMusicOn);
    }
}
