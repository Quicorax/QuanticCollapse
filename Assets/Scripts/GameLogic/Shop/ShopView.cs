using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuanticCollapse
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _dilithium_Text;
        [SerializeField]
        private TMP_Text _allianceCredits_Text;
        [SerializeField]
        private TMP_Text _reputation_Text;
        [SerializeField]
        private TMP_Text _fistAid_Text;
        [SerializeField]
        private TMP_Text _easyTrigger_Text;
        [SerializeField]
        private TMP_Text _deAthomizer_Text;

        [SerializeField]
        private ShopController _shopController;
        [SerializeField]
        private RectTransform _parent;

        [SerializeField]
        private Button _rewardedAdButton;

        private GameProgressionService _gameProgression;
        private AddressablesService _addressables;
        private LocalizationService _localization;
        private GameConfigService _gameConfig;
        private IAPGameService _gameIAP;
        private PopUpService _popUps;

        private List<string> _productSectionAdded = new();
        private IAPBundle _iapBundleOnSight = null;

        private void Awake()
        {
            _gameProgression = ServiceLocator.GetService<GameProgressionService>();
            _addressables = ServiceLocator.GetService<AddressablesService>();
            _localization = ServiceLocator.GetService<LocalizationService>();
            _gameConfig = ServiceLocator.GetService<GameConfigService>();
            _gameIAP = ServiceLocator.GetService<IAPGameService>();
            _popUps = ServiceLocator.GetService<PopUpService>();
        }
        private void Start()
        {
            Initialize();
            UpdateInventoryVisualAmount();
        }
        public void Initialize()
        {
            _shopController = new();

            foreach (ShopElementModel shopElements in _gameConfig.ShopModel)
            {
                if (!_productSectionAdded.Contains(shopElements.Product.Id))
                {
                    _productSectionAdded.Add(shopElements.Product.Id);

                    _addressables.LoadAdrsOfComponent<ShopSectionView>("ShopSection", _parent, x =>
                    {
                        x.InitProductSection(
                            shopElements.Product.Id,
                            _gameConfig.ShopModel,
                            TryPurchaseProduct,
                            transform);
                    });

                    _parent.sizeDelta += new Vector2(0, 1150f);
                }
            }
        }

        public void TryPurchaseProduct(ShopElementModel transactionData)
        {
            if (_gameProgression.CheckElement(transactionData.Price.Id) >= transactionData.Price.Amount)
                _shopController.PurchaseElement(transactionData, UpdateInventoryVisualAmount);
            else
                NotEnoughtResourcesPopUp(transactionData.Price.Id);
        }

        #region IAP
        public void PurchaseIAPProduct(string productName)
        {
            foreach (IAPBundle product in _gameConfig.IAPProducts)
            {
                if (product.ProductName == productName)
                {
                    _iapBundleOnSight = product;

                    BuyInAppProductPopUp(product);
                    break;
                }
            }
        }
        private void TryPurchaseIAPProduct()
        {
            CallIAP(_iapBundleOnSight).ManageTaskExeption();
            _iapBundleOnSight = null;
        }
        private async Task CallIAP(IAPBundle product)
        {
            if (await _gameIAP.StartPurchase(product.ProductName))
            {
                _shopController.PurchaseElementWithIAP(product, UpdateInventoryVisualAmount);
                UpdateInventoryVisualAmount();
            }
            else
                IAPFailedPopUp();
        }
        #endregion

        #region Rewarded Ad
        public void PurchaseGoldFromRewardedAd() => AskGoldFromRewardedAd().ManageTaskExeption();

        private async Task AskGoldFromRewardedAd()
        {
            if (await ServiceLocator.GetService<AdsGameService>().ShowAd())
            {
                _shopController.PurchaseElementWithRewardedAdd(
                    _gameConfig.VideoAddRewards.AllianceCredits,
                    UpdateInventoryVisualAmount);
            }
        }
        private void UpdateInventoryVisualAmount()
        {
            _dilithium_Text.text = _gameProgression.CheckElement("Dilithium").ToString();
            _allianceCredits_Text.text = _gameProgression.CheckElement("AllianceCredits").ToString();
            _reputation_Text.text = _gameProgression.CheckElement("Reputation").ToString();
            _fistAid_Text.text = _gameProgression.CheckElement("FirstAidKit").ToString();
            _easyTrigger_Text.text = _gameProgression.CheckElement("EasyTrigger").ToString();
            _deAthomizer_Text.text = _gameProgression.CheckElement("DeAthomizer").ToString();
        }
        #endregion

        #region PopUps
        private void NotEnoughtResourcesPopUp(string resourceId)
        {
            _popUps.SpawnPopUp(transform.parent, new IPopUpComponentData[]
            {
                _popUps.AddHeader(_localization.Localize("LOBBY_MAIN_NOTENOUGHT"), true),
                _popUps.AddImage(resourceId, string.Empty),
                _popUps.AddCloseButton(),
            });
        }

        private void IAPFailedPopUp()
        {
            _popUps.SpawnPopUp(transform.parent, new IPopUpComponentData[]
            {
                _popUps.AddHeader(_localization.Localize("LOBBY_SHOP_IAPFAILED_HEADER"), true),
                _popUps.AddImage("Skull", string.Empty),
                _popUps.AddText(_localization.Localize("LOBBY_SHOP_IAPFAILED_BODY")),
                _popUps.AddCloseButton(),
            });
        }
        private void BuyInAppProductPopUp(IAPBundle product)
        {
            _popUps.SpawnPopUp(transform.parent, new IPopUpComponentData[]
            {
                _popUps.AddHeader(_iapBundleOnSight.ProductName, true),
                _popUps.AddImage(product.Product.Id, "x" + _iapBundleOnSight.Product.Amount),
                _popUps.AddText(_gameIAP.GetRemotePrice(product.ProductName)),
                _popUps.AddButton(_localization.Localize("LOBBY_MAIN_BUY"), TryPurchaseIAPProduct, true),
                _popUps.AddCloseButton(),
            });
        }

        #endregion
    }
}