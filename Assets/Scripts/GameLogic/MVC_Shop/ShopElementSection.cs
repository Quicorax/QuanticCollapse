using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using System;

public class ShopElementSection : MonoBehaviour
{
    const string ShopProductAdrsKey = "ProductSample";

    [SerializeField] private TMP_Text productHeader;
    [SerializeField] private RectTransform _parent;

    private Action<ShopElementModel> _purchaseAction;

    private ShopElementModel transactionOnSight;
    public void InitProductSection(string productKind, List<ShopElementModel> ShopElements, Action<ShopElementModel> purchaseAction)
    {
        productHeader.text = productKind;
        _purchaseAction = purchaseAction;

        foreach (ShopElementModel shopElements in ShopElements)
        {
            if (shopElements.ProductKind == productKind)
            {
                Addressables.LoadAssetAsync<GameObject>(ShopProductAdrsKey).Completed += handle =>
                {
                    GameObject element = Addressables.InstantiateAsync(ShopProductAdrsKey, _parent).Result;
                    element.name = shopElements.ProductName;
                    element.GetComponent<ShopElement>().InitProduct(shopElements, BuyProduct);

                    _parent.sizeDelta += new Vector2(270f, 0);
                };
            }
        }
    }

    void BuyProduct(ShopElementModel transactionData)
    {
        transactionOnSight = transactionData;

        SpawnPopUp popUp = new SpawnPopUp(transform.parent.parent.parent);

        PopUpData data = new();
        data.SetHeader(transactionData.ProductName, true);
        data.SetIcon(transactionData.ProductImage);
        data.SetBodyText(transactionData.ProductBody);
        data.SetButton("Buy Product", TryPurchase);
        data.SetPriceTag(transactionData.PriceAmount);
        data.SetCloseButton();

        popUp.GeneratePopUp(data, false);
    }
    void TryPurchase()
    {
        _purchaseAction?.Invoke(transactionOnSight);
        transactionOnSight = null;
    }
}
