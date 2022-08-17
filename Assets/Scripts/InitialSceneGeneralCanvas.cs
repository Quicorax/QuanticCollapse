using UnityEngine;
using DG.Tweening;
using TMPro;

public class InitialSceneGeneralCanvas : MonoBehaviour
{
    CanvasGroup canvasGroup;

    public Transform optionsIcon;
    public Transform missionLog;
    public Transform shopIcon;

    float optionsIconInitialX;
    float missionLogInitialX;
    float shopIconInitialY;

    public float optionsIconOffetX;
    public float missionLogOffetX;
    public float shopIconOffetY;

    public TMP_Text dilithium_Text;
    public TMP_Text alianceCredits_Text;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        optionsIconInitialX = optionsIcon.position.x;
        missionLogInitialX = missionLog.position.x;
        shopIconInitialY = shopIcon.position.y;

        HideAllElements(false);
    }
    public void HideAllElements(bool hide)
    {
        canvasGroup.DOFade(hide ? 0 : 1, 0.5f);
        optionsIcon.DOMoveX(hide ? optionsIconInitialX + optionsIconOffetX : optionsIconInitialX, 0.5f);
        missionLog.DOMoveX(hide ? missionLogInitialX + missionLogOffetX : missionLogInitialX, 0.5f);
        shopIcon.DOMoveY(hide ? shopIconInitialY + shopIconOffetY : shopIconInitialY, 0.5f);
    }

    public void SetDilithiumAmount(int amount)
    {
        dilithium_Text.text = amount.ToString();
    }
    public void SetCreditsAmount(int amount)
    {
        alianceCredits_Text.text = amount.ToString();
    }
}
