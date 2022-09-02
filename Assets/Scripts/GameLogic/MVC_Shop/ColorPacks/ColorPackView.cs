using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPackView : MonoBehaviour
{
    private ShopElementModel _transactionData;
    public ColorPackModel ColorPackModel;


    [SerializeField] private TMP_Text _productHeader;
    [SerializeField] private Image[] _productColors;

    private Action<ShopElementModel> _transaction;
    public void InitColorPack(ShopElementModel transactionData, Action<ShopElementModel> transaction)
    {
        _transactionData = transactionData;
        _productHeader.text = transactionData.ProductName;

        Color[] colorPack = new Color().GenerateColorPackFromFormatedString(transactionData.ProductImage);
        ColorPackModel = new(colorPack);

        _productColors[0].color = colorPack[0];
        _productColors[1].color = colorPack[1];
        _productColors[2].color = colorPack[2];

        _transaction = transaction;
    }
    public void Purchase() => _transaction?.Invoke(_transactionData);
}