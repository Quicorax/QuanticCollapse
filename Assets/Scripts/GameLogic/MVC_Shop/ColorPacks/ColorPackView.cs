using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPackView : MonoBehaviour
{
    private ShopElementModel _transactionData;

    [SerializeField] private TMP_Text _productHeader;
    [SerializeField] private Image[] _productColors;



    private Action<ShopElementModel> _transaction;
    public void InitColorPack(ShopElementModel transactionData, Action<ShopElementModel> transaction)
    {
        _transactionData = transactionData;
        _productHeader.text = transactionData.ProductName;

        string[] splitColors = transactionData.ProductImage.Split("_");

        string[] splitColorChannelsPrimary = splitColors[0].Split("-");
        _productColors[0].color = new Color(float.Parse(splitColorChannelsPrimary[0]), float.Parse(splitColorChannelsPrimary[1]), float.Parse(splitColorChannelsPrimary[2]));
        Debug.Log(float.Parse(splitColorChannelsPrimary[0]) +" : "+ float.Parse(splitColorChannelsPrimary[1]) +" : "+ float.Parse(splitColorChannelsPrimary[2]));

        string[] splitColorChannelsSecondary = splitColors[1].Split("-");
        _productColors[1].color = new Color(float.Parse(splitColorChannelsSecondary[0]), float.Parse(splitColorChannelsSecondary[1]), float.Parse(splitColorChannelsSecondary[2]));

        string[] splitColorChannelsSignature = splitColors[2].Split("-");
        _productColors[2].color = new Color(float.Parse(splitColorChannelsSignature[0]), float.Parse(splitColorChannelsSignature[1]), float.Parse(splitColorChannelsSignature[2]));
        _transaction = transaction;
    }

    public void Purchase() => _transaction?.Invoke(_transactionData);
}