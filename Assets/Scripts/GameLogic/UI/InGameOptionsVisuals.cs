using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InGameOptionsVisuals : MonoBehaviour
{
    [SerializeField] private GameplaySceneGeneralCanvas _canvas;
    [SerializeField] private Toggle optionsToggle;
    [SerializeField] private float panelLateralOffset;

    private CanvasGroup canvasGroup;

    private float originalX;
    private float hiddenX;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Start()
    {
        this.gameObject.SetActive(false);
        canvasGroup.alpha = 0;
        originalX = transform.GetChild(0).position.x;

        hiddenX = originalX + panelLateralOffset;

        transform.GetChild(0).DOMoveX(hiddenX, 0);
    }

    public void TurnOptionsPanel(bool on)
    {
        optionsToggle.interactable = false;

        if (on)
            gameObject.SetActive(true);

        canvasGroup.DOFade(on ? 1 : 0, .25f);
        transform.GetChild(0).DOMoveX(on ? originalX : hiddenX, 0.5f).OnComplete(() =>
        {
            if(!on) 
                gameObject.SetActive(false);

            optionsToggle.interactable = true;
        });
    }

    public void OpenExitPopUp()
    {
        SpawnPopUp popUp = new SpawnPopUp(transform);
        popUp.SimpleGeneratePopUp("EscapeMission", Retreat);
    }

    void Retreat() => _canvas.RetreatFromMission();
}
