using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using System;

public class ShopElementSection : MonoBehaviour
{
    const string ShopProductAdrsKey = "ProductSample";
    const string ShopColorsAdrsKey = "ColorPack";

    [SerializeField] private TMP_Text productHeader;
    [SerializeField] private Transform _parent;

    public List<GameObject> products = new();
    private int totalExpectedProducts;
    private int productOnDisplayIndex;

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
                totalExpectedProducts++;

                Addressables.LoadAssetAsync<GameObject>(ShopProductAdrsKey).Completed += handle =>
                {
                    GameObject element = Addressables.InstantiateAsync(ShopProductAdrsKey, _parent).Result;
                    element.name = shopElements.ProductName;
                    element.GetComponent<ShopElement>().InitProduct(shopElements, BuyProduct);
                    products.Add(element);

                    if (products.Count == totalExpectedProducts)
                        SetInitialProduct();
                };

                //Addressables.LoadAssetAsync<GameObject>(ShopColorsAdrsKey).Completed += handle =>
                //{
                //    GameObject element = Addressables.InstantiateAsync(ShopColorsAdrsKey, _parent).Result;
                //    element.name = shopElements.ProductName;
                //    element.GetComponent<ColorPackView>().InitColorPack(shopElements, BuyProduct);
                //    products.Add(element);
                //
                //    if (products.Count == totalExpectedProducts)
                //        SetInitialProduct();
                //};
            }
        }
    }

    private void DisplayProduct()
    {
        for (int i = 0; i < products.Count; i++)
        {
            products[i].SetActive(i == productOnDisplayIndex);
        }
    }
    void BuyProduct(ShopElementModel transactionData)
    {
        transactionOnSight = transactionData;

        SpawnPopUp popUp = new SpawnPopUp(transform.parent.parent);

        PopUpData data = new();
        data.SetHeader(transactionData.ProductName, true);

        if(transactionData.ProductKind != "ColorSkin")
            data.SetIcon(transactionData.ProductImage);

        data.SetBodyText(transactionData.ProductBody);
        data.SetButton("Buy Product", TryPurchase);
        data.SetCloseButton();

        popUp.GeneratePopUp(data, false);
    }
    void TryPurchase()
    {
        _purchaseAction?.Invoke(transactionOnSight);
        transactionOnSight = null;
    }
    void SetInitialProduct()
    {
        productOnDisplayIndex = 0;
        DisplayProduct();
    }
    public void SetNextProduct()
    {
        productOnDisplayIndex++;

        if (productOnDisplayIndex >= products.Count)
            productOnDisplayIndex = 0;

        DisplayProduct();
    }

    public void SetPreviousProduct()
    {
        productOnDisplayIndex--;

        if (productOnDisplayIndex < 0 )
            productOnDisplayIndex = products.Count -1;

        DisplayProduct();

    }
}
