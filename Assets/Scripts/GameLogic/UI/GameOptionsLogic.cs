using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class GameOptionsLogic : MonoBehaviour
{
    [SerializeField] private GameplayCanvasManager _canvas;
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
        gameObject.SetActive(false);
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
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData("Escape", true),
            new ImagePopUpComponentData("Skull"),
            new TextPopUpComponentData("You will lose the mission progress"),
            new ButtonPopUpComponentData("Confirm Exit", Retreat, true),
            new CloseButtonPopUpComponentData()
        };

        Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
        {
            Addressables.InstantiateAsync("Modular_PopUp", transform.parent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }

    void Retreat() => _canvas.RetreatFromMission();
}
