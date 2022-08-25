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

    private ShopElementModel _elementModel;
    private Action<ShopElementModel> _onElementClicked;

    public void InitElement(ShopElementModel model, Action<ShopElementModel> elementClockedEvent)
    {
        _onElementClicked = elementClockedEvent;
        _elementModel = model;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (_elementModel == null)
            return;

        _productImage.sprite = _sprites.Find(sprite => sprite.name == _elementModel.ProductKind);
        _productHeader.text = _elementModel.ProductKind;
        _productAmount.text = _elementModel.ProductAmount.ToString();
        _productBody.text = _elementModel.ProductBody;

        _priceImage.sprite = _sprites.Find(sprite => sprite.name == _elementModel.PriceKind);
        _priceAmount.text = _elementModel.PriceAmount.ToString();
    }

    public void BuyElement()
    {
        if (_elementModel == null)
            return;

        _onElementClicked?.Invoke(_elementModel);
    }
}
