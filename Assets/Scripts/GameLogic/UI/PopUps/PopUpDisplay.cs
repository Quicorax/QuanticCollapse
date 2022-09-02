using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpDisplay : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new();

    [SerializeField] private GameObject highlightedHeader;
    [SerializeField] private GameObject basicHeader;
    [SerializeField] private GameObject bottomButton;
    [SerializeField] private GameObject basicBody;
    [SerializeField] private GameObject iconImage;
    [SerializeField] private GameObject closeButton;

    public PopUpData data;

    public void Initialize(PopUpData popUpData)
    {
        data = popUpData;

        float popUpHeight = 140 
            + (data.HasIcon ? iconImage.GetComponent<RectTransform>().sizeDelta.y + 30 : 0) 
            + (data.HasBodyText ? 110 : 0) 
            + (data.HasButton ? 40 : 0);

        GetComponent<RectTransform>().sizeDelta = new Vector2(350, popUpHeight);

        highlightedHeader.SetActive(data.HasHighlightedHeader);
        basicHeader.SetActive(data.HasBasicHeader);
        bottomButton.SetActive(data.HasButton);
        iconImage.SetActive(data.HasIcon);
        basicBody.SetActive(data.HasBodyText);
        closeButton.SetActive(data.HasCloseButton);

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
            iconImage.transform.position += Vector3.up * (data.HasButton ? 170 : 70);
        }

        if (data.HasCloseButton)
        {
            closeButton.GetComponent<Button>().interactable = true;
        }
    }

    public void ButtonAction()
    {
        data.onButtonClickedAction?.Invoke();
        CloseAction();
    }

    public void CloseAction()
    {
        data.onCloseButtonClickedAction?.Invoke();
    }

}
