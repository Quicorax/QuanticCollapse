using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopElementView : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new();
    [HideInInspector] public ShopElementModel TransactionData;

    private Action<ShopElementModel> _transaction;
    public void InitProduct(ShopElementModel transactionData, Action<ShopElementModel> transaction)
    {
        TransactionData = transactionData;

        GetComponent<Image>().sprite = _sprites.Find(sprite => sprite.name == transactionData.ProductImage);

        _transaction = transaction;
    }

    public void Purchase() => _transaction?.Invoke(TransactionData);
}
