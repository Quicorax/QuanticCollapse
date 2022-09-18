using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShopElementSection : MonoBehaviour
{
    [SerializeField] private TMP_Text productHeader;
    [SerializeField] private RectTransform _elementParent;
    private Transform _sectionParent;

    private Action<ShopElementModel> _purchaseAction;

    private ShopElementModel _transactionOnSight;
    private AddressablesService _addressables;
    private void Awake()
    {
        _addressables = ServiceLocator.GetService<AddressablesService>();
    }

    public void InitProductSection(string productKind, List<ShopElementModel> ShopElements, Action<ShopElementModel> purchaseAction, Transform sectionParent)
    {
        productHeader.text = productKind;
        _purchaseAction = purchaseAction;
        _sectionParent = sectionParent;

        foreach (ShopElementModel shopElements in ShopElements)
        {
            if (shopElements.ProductKind == productKind)
            {
                _addressables.SpawnAddressable<ShopElement>(Constants.ShopProduct, _elementParent, x=> x.InitProduct(shopElements, BuyProduct));

                _elementParent.sizeDelta += new Vector2(270f, 0);
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
            new ImagePopUpComponentData(transactionData.ProductImage, Constants.X + transactionData.ProductAmount),
            new PricePopUpComponentData(transactionData.PriceAmount.ToString()),
            new ButtonPopUpComponentData(Constants.Buy, TryPurchase, true),
            new CloseButtonPopUpComponentData()
        };

        ServiceLocator.GetService<PopUpService>().SpawnPopUp(Modules, _sectionParent);
    }
    void TryPurchase()
    {
        _purchaseAction?.Invoke(_transactionOnSight);
        _transactionOnSight = null;
    }
}
