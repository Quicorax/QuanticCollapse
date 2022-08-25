using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopElementView : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();

    [SerializeField] private Image _productImage;
    [SerializeField] private TMP_Text _productHeader;
    [SerializeField] private TMP_Text _productAmount;
    [SerializeField] private TMP_Text _productBody;

    [SerializeField] private Image _priceImage;
    [SerializeField] private TMP_Text _priceAmount;

    public ShopElementModel ElementModel;
    private Action<ShopElementModel> _onElementClicked;

    public void InitElement(ShopElementModel model, Action<ShopElementModel> elementClockedEvent)
    {
        _onElementClicked = elementClockedEvent;
        ElementModel = model;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (ElementModel == null)
            return;

        _productImage.sprite = _sprites.Find(sprite => sprite.name == ElementModel.ProductKind);
        _productHeader.text = ElementModel.ProductKind;
        _productAmount.text = ElementModel.ProductAmount.ToString();
        _productBody.text = ElementModel.ProductBody;

        _priceImage.sprite = _sprites.Find(sprite => sprite.name == ElementModel.PriceKind);
        _priceAmount.text = ElementModel.PriceAmount.ToString();
    }

    public void BuyElement()
    {
        if (ElementModel == null)
            return;

        _onElementClicked?.Invoke(ElementModel);
    }
}
