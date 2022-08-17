using UnityEngine;
using DG.Tweening;
using TMPro;

public class InitialSceneGeneralCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup initialCanvasGroup;
    [SerializeField] private CanvasGroup shopCanvasGroup;
    [SerializeField] private CanvasGroup persistentCanvasGroup;

    [SerializeField] private Transform shopCanvas;

    [SerializeField] private Transform missionLog;
    [SerializeField] private Transform shopIcon;

    float shopIconInitialY;

    [SerializeField] private TMP_Text dilithium_Text;
    [SerializeField] private TMP_Text alianceCredits_Text;

    private void Start()
    {
        shopIconInitialY = shopIcon.position.y;

        HideShopElemennts(true);
    }

    public void CanvasEngageTrigger(bool hide)
    {
        HideAllInitialElements(hide);
        persistentCanvasGroup.DOFade(hide ? 0 : 1, 0.5f);
    }

    public void SetDilithiumAmount(int amount) { dilithium_Text.text = amount.ToString(); }
    public void SetCreditsAmount(int amount) { alianceCredits_Text.text = amount.ToString(); }

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
}
