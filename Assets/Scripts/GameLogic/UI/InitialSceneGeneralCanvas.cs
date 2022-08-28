using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InitialSceneGeneralCanvas : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;

    private MasterSceneManager _MasterSceneManager;

    [SerializeField] private CanvasGroup initialCanvasGroup;
    [SerializeField] private CanvasGroup persistentCanvasGroup;

    [SerializeField] private Transform _shopView;

    [SerializeField] private Transform missionLog;
    [SerializeField] private Transform shopIcon;

    [SerializeField] private Toggle toggleSFX;
    [SerializeField] private Toggle toggleMusic;

    float shopIconInitialY;

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

        HideShopElements(true);

        toggleSFX.isOn = !_MasterSceneManager.SaveFiles.configuration.isSFXOn;
        toggleMusic.isOn = !_MasterSceneManager.SaveFiles.configuration.isMusicOn;
    }

    void SetMasterReference(MasterSceneManager masterReference) { _MasterSceneManager = masterReference; }
    public void CanvasEngageTrigger(bool hide)
    {
        HideAllInitialElements(hide);
        persistentCanvasGroup.DOFade(hide ? 0 : 1, 0.5f);
    }

    void HideAllInitialElements(bool hide)
    {
        initialCanvasGroup.DOFade(hide ? 0 : 1, 0.5f);

        missionLog.DOMoveX(hide ? Screen.width : -Screen.width, 0.5f).SetRelative();
        shopIcon.DOMoveY(hide ? shopIconInitialY - 300 : shopIconInitialY, 0.5f);
    }
    void HideShopElements(bool hide)
    {
        _shopView.GetComponent<CanvasGroup>().DOFade(hide ? 0 : 1, 0.5f);
        _shopView.DOMoveX(hide ? -Screen.width : Screen.width, 0.5f).SetRelative();
    }
    public void TransitionToShopCanvas()
    {
        HideAllInitialElements(true);
        HideShopElements(false);
    }
    public void TransitionToInitialCanvas()
    {
        HideAllInitialElements(false);
        HideShopElements(true);
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
