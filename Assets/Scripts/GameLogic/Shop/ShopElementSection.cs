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
    private LocalizationService _localization;

    private void Awake()
    {
        _addressables = ServiceLocator.GetService<AddressablesService>();
        _localization = ServiceLocator.GetService<LocalizationService>();
    }

    public void InitProductSection(string productKind, List<ShopElementModel> ShopElements, Action<ShopElementModel> purchaseAction, Transform sectionParent)
    {
        productHeader.text = _localization.Localize(productKind);
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
        PurchaseInGamePopUp(transactionData);
    }

    void PurchaseInGamePopUp(ShopElementModel transactionData)
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData(_localization.Localize(transactionData.ProductName), true),
            new TextPopUpComponentData(_localization.Localize(transactionData.ProductBody)),
            new ImagePopUpComponentData(transactionData.ProductImage, Constants.X + transactionData.ProductAmount),
            new PricePopUpComponentData(transactionData.PriceAmount.ToString()),
            new ButtonPopUpComponentData(_localization.Localize("LOBBY_MAIN_BUY"), TryPurchase, true),
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
