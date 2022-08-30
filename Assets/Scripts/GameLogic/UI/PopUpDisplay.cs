using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpData
{
    public bool HasHighlightedHeader = false;
    public bool HasBasicHeader = false;
    public string HeaderText;

    public bool HasButton = false;
    public string ButtonText;
    public Action onButtonClickedAction;

    public bool HasIcon = false;
    public string IconName;

    public bool HasBodyText = false;
    public string BodyText;

    public void SetHeader(string text, bool isHighlighted)
    {
        if (isHighlighted)
            HasHighlightedHeader = true;
        else
            HasBasicHeader = true;

        HeaderText = text;
    }

    public void SetButton(string text, Action action)
    {
        HasButton = true;
        ButtonText = text;
        onButtonClickedAction = action;
    }

    public void SetIcon(string text)
    {
        HasIcon = true;
        IconName = text;
    }
    public void SetBodyText(string text)
    {
        HasBodyText = true;
        BodyText = text;
    }
}

public class PopUpDisplay : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new();

    [SerializeField] private GameObject highlightedHeader;
    [SerializeField] private GameObject basicHeader;
    [SerializeField] private GameObject bottomButton;
    [SerializeField] private GameObject basicBody;
    [SerializeField] private GameObject iconImage;

    public PopUpData data;

    public void Initialize(PopUpData popUpData)
    {
        data = popUpData;

        float popUpHeight = 130 
            + (data.HasIcon ? iconImage.GetComponent<RectTransform>().sizeDelta.y + 30 : 0) 
            + (data.HasBodyText ? 110 : 0) 
            + (data.HasButton ? 40 : 0);

        GetComponent<RectTransform>().sizeDelta = new Vector2(350, popUpHeight);

        highlightedHeader.SetActive(data.HasHighlightedHeader);
        basicHeader.SetActive(data.HasBasicHeader);
        bottomButton.SetActive(data.HasButton);
        iconImage.SetActive(data.HasIcon);
        basicBody.SetActive(data.HasBodyText);

        if (data.HasHighlightedHeader)
            highlightedHeader.GetComponentInChildren<TMP_Text>().text = data.HeaderText;
        if(data.HasBasicHeader)
            basicHeader.GetComponentInChildren<TMP_Text>().text = data.HeaderText;

        if (data.HasButton)
            bottomButton.GetComponentInChildren<TMP_Text>().text = data.ButtonText;

        if (data.HasBodyText)
            basicBody.GetComponent<TMP_Text>().text = data.BodyText;

        if (data.HasIcon)
        {
            iconImage.GetComponent<Image>().sprite = sprites.Find(img => img.name == data.IconName);
            iconImage.transform.position += Vector3.up * (data.HasButton ? 220 : 100);
        }

    }

    public void ButtonAction()
    {
        data.onButtonClickedAction?.Invoke();
    }
}
