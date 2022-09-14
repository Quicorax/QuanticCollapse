using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new();
    [HideInInspector] public ShopElementModel transactionData;

    private Image _productImage;

    private Action<ShopElementModel> _transaction;
    public void InitProduct(ShopElementModel transactionData, Action<ShopElementModel> transaction)
    {
        this.transactionData = transactionData;

        _productImage = GetComponent<Image>();
        _productImage.sprite = _sprites.Find(sprite => sprite.name == transactionData.ProductImage);

        _transaction = transaction;
    }

    public void Purchase() => _transaction?.Invoke(transactionData);
}
