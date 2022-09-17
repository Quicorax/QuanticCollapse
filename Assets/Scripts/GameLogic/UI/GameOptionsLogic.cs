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
            new HeaderPopUpComponentData(Constants.Escape, true),
            new ImagePopUpComponentData(Constants.SkullIcon),
            new TextPopUpComponentData(Constants.EscapeLog),
            new ButtonPopUpComponentData(Constants.ConfirmEscape, Retreat, true),
            new CloseButtonPopUpComponentData()
        };

        Addressables.LoadAssetAsync<GameObject>(Constants.ModularPopUp).Completed += handle =>
        {
            Addressables.InstantiateAsync(Constants.ModularPopUp, transform.parent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }

    async void Retreat() => _canvas.RetreatFromMission();

    public void ShowCredits()
    {
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData(Constants.Credits, true),
            new TextPopUpComponentData(Constants.CreditsSelf),
            new TextPopUpComponentData(Constants.CreditsLog),
            new TextPopUpComponentData(Constants.Kenney),
            new TextPopUpComponentData(Constants.Quaternius),
            new TextPopUpComponentData(Constants.Iconian),
            new CloseButtonPopUpComponentData()
        };

        Addressables.LoadAssetAsync<GameObject>(Constants.ModularPopUp).Completed += handle =>
        {
            Addressables.InstantiateAsync(Constants.ModularPopUp, transform.parent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }

    public void DeleteLocalFiles()
    {
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData(Constants.DeleteFiles, true),
            new TextPopUpComponentData(Constants.DeleteFilesLog),
            new TextPopUpComponentData(Constants.DeleteFilesDetail),
            new TextPopUpComponentData(Constants.DeleteFilesSpecific),
            new ButtonPopUpComponentData(Constants.DeleteFiles, ConfirmDeleteFiles, true),
            new CloseButtonPopUpComponentData()
        };

        Addressables.LoadAssetAsync<GameObject>(Constants.ModularPopUp).Completed += handle =>
        {   
            Addressables.InstantiateAsync(Constants.ModularPopUp, transform.parent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }

    void ConfirmDeleteFiles()
    {
        ServiceLocator.GetService<SaveLoadService>().DeleteLocalFiles();
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
