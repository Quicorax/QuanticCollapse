using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StarshipColorsView : MonoBehaviour
{
    [SerializeField] private TMP_Text PackHeaderText;
    [SerializeField] private Image[] PackColorsDisplay;

    [SerializeField] private GameObject lockImage;

    private DeSeializedStarshipColors _colorsSkin;

    private Action<DeSeializedStarshipColors, Action> interactEvent;
    public void InitStarshipColorView(DeSeializedStarshipColors skin, bool isLocked, Action<DeSeializedStarshipColors, Action> onInteract)
    {
        _colorsSkin = skin;
        interactEvent = onInteract;
        lockImage.SetActive(isLocked);
        PackHeaderText.text = _colorsSkin.SkinName;

        for (int i = 0; i < _colorsSkin.SkinColors.Length; i++)
            PackColorsDisplay[i].color = _colorsSkin.SkinColors[i];
    }

    public void InteractWithColor() => interactEvent?.Invoke(_colorsSkin, PurchaseConfirmation);
    void PurchaseConfirmation() => lockImage.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() => lockImage.SetActive(false));
}

