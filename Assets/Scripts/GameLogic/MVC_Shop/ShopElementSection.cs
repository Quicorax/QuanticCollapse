using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using System;

public class ShopElementSection : MonoBehaviour
{
    const string ShopProductAdrsKey = "ProductSample";

    [SerializeField] private TMP_Text productHeader;
    [SerializeField] private RectTransform _elementParent;
    private Transform _sectionParent;

    private Action<ShopElementModel> _purchaseAction;

    private ShopElementModel _transactionOnSight;
    public void InitProductSection(string productKind, List<ShopElementModel> ShopElements, Action<ShopElementModel> purchaseAction, Transform sectionParent)
    {
        productHeader.text = productKind;
        _purchaseAction = purchaseAction;
        _sectionParent = sectionParent;

        foreach (ShopElementModel shopElements in ShopElements)
        {
            if (shopElements.ProductKind == productKind)
            {
                Addressables.LoadAssetAsync<GameObject>(ShopProductAdrsKey).Completed += handle =>
                {
                    GameObject element = Addressables.InstantiateAsync(ShopProductAdrsKey, _elementParent).Result;
                    element.name = shopElements.ProductName;
                    element.GetComponent<ShopElement>().InitProduct(shopElements, BuyProduct);

                    _elementParent.sizeDelta += new Vector2(270f, 0);
                };
            }
        }
    }

    void BuyProduct(ShopElementModel transactionData)
    {
        _transactionOnSight = transactionData;

        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData(transactionData.ProductName, true),
            new TextPopUpComponentData(transactionData.ProductBody),
            new ImagePopUpComponentData(transactionData.ProductImage, "x"+transactionData.ProductAmount),
            new PricePopUpComponentData(transactionData.PriceAmount.ToString()),
            new ButtonPopUpComponentData("Buy", TryPurchase, true),
            new CloseButtonPopUpComponentData()
        };

        Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
        {
            Addressables.InstantiateAsync("Modular_PopUp", _sectionParent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }
    void TryPurchase()
    {
        _purchaseAction?.Invoke(_transactionOnSight);
        _transactionOnSight = null;
    }
}
