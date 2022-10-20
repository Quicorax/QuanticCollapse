using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace QuanticCollapse
{
    public class ShopSectionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text productHeader;
        [SerializeField] private RectTransform _elementParent;
        private Transform _sectionParent;

        private Action<ShopElementModel> _purchaseAction;

        private ShopElementModel _transactionOnSight;
        private AddressablesService _addressables;
        private LocalizationService _localization;
        private PopUpService _popUps;

        private void Awake()
        {
            _addressables = ServiceLocator.GetService<AddressablesService>();
            _localization = ServiceLocator.GetService<LocalizationService>();
            _popUps = ServiceLocator.GetService<PopUpService>();

        }

        public void InitProductSection(string productName, List<ShopElementModel> ShopElements, Action<ShopElementModel> purchaseAction, Transform sectionParent)
        {
            productHeader.text = _localization.Localize(productName);
            _purchaseAction = purchaseAction;
            _sectionParent = sectionParent;

            foreach (ShopElementModel shopElements in ShopElements)
            {
                if (shopElements.Product.Id == productName)
                {
                    _addressables.LoadAdrsOfComponent<ShopElementView>("SectionProduct", 
                        _elementParent, x => x.InitProduct(shopElements, BuyProduct).ManageTaskExeption());

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

            _popUps.SpawnPopUp(_sectionParent, new IPopUpComponentData[]
            {
            _popUps.AddHeader(_localization.Localize(transactionData.Product.Id), true),
            _popUps.AddText(_localization.Localize(transactionData.ProductBody)),
            _popUps.AddImage(transactionData.ProductImage, "x" + transactionData.Product.Amount),
            _popUps.AddPrice(transactionData.Price.Amount.ToString()),
            _popUps.AddButton(_localization.Localize("LOBBY_MAIN_BUY"), TryPurchase, true),
            _popUps.AddCloseButton(),
            });
        }
        void TryPurchase()
        {
            _purchaseAction?.Invoke(_transactionOnSight);
            _transactionOnSight = null;
        }
    }
}