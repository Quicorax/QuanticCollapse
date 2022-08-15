using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InGameOptionsVisuals : MonoBehaviour
{
    public Toggle optionsToggle;
    public float panelLateralOffset;
    CanvasGroup canvasGroup;

    float originalX;
    float hiddenX;

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
}
