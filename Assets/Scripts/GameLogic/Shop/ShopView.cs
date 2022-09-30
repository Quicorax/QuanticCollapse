using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [SerializeField] private TMP_Text _dilithium_Text;
    [SerializeField] private TMP_Text _allianceCredits_Text;
    [SerializeField] private TMP_Text _reputation_Text;
    [SerializeField] private TMP_Text _fistAid_Text;
    [SerializeField] private TMP_Text _easyTrigger_Text;
    [SerializeField] private TMP_Text _deAthomizer_Text;

    [SerializeField] private ShopController _shopController;
    [SerializeField] private RectTransform _parent;

    [SerializeField] private Button _rewardedAdButton;

    private GameProgressionService _gameProgress;
    private AddressablesService _addressables;
    private LocalizationService _localization;
    private GameConfigService _gameConfig;
    private IAPGameService _gameIAP;
    private PopUpService _popUps;

    private List<string> _productSectionAdded = new();
    private IAPBundle _iapBundleOnSight = null;

    private void Awake()
    {
        _gameProgress = ServiceLocator.GetService<GameProgressionService>();
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

        foreach (ShopElementModel shopElements in _shopController.ShopModel.ShopElements)
        {
            if (!_productSectionAdded.Contains(shopElements.ProductKind))
            {
                _productSectionAdded.Add(shopElements.ProductKind);

                _addressables.SpawnAddressable<ShopElementSection>("ProductSection", _parent, x => 
                { 
                    x.InitProductSection(shopElements.ProductKind, _shopController.ShopModel.ShopElements, TryPurchaseProduct, transform);
                });
                
                _parent.sizeDelta += new Vector2(0, 1150f);
            }
        }
    }

    public void TryPurchaseProduct(ShopElementModel transactionData)
    {
        if (_gameProgress.CheckElement(ResourcesType.AllianceCredits) >= transactionData.PriceAmount)
            _shopController.PurchaseElement(transactionData, UpdateInventoryVisualAmount);
        else
            NotEnoughtCredits();
    }
    public void PurchaseIAPProduct(string productName) 
    {
        foreach (IAPBundle product in _gameConfig.AllianceCreditsPerIAP)
        {
            if (product.ProductName == productName)
            {
                _iapBundleOnSight = product;
                break;
            }
        }

        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData(_iapBundleOnSight.ProductName, true),
            new ImagePopUpComponentData("AllianceCredits", "x" + _iapBundleOnSight.ProductAmount),
            new TextPopUpComponentData(_gameIAP.GetRemotePrice(productName)),
            new ButtonPopUpComponentData(_localization.Localize("LOBBY_MAIN_BUY"), TryPurchaseIAPProduct, true),
            new CloseButtonPopUpComponentData()
        };

        _popUps.SpawnPopUp(Modules, transform.parent);
    } 
    void TryPurchaseIAPProduct()
    {
        CallThirdTardyPurchase(_iapBundleOnSight).ManageTaskExeption();
        _iapBundleOnSight = null;
    }
    async Task CallThirdTardyPurchase(IAPBundle product)
    {
        if (await _gameIAP.StartPurchase(product.ProductName))
        {
            ServiceLocator.GetService<GameProgressionService>().UpdateElement(ResourcesType.AllianceCredits, product.ProductAmount);
            UpdateInventoryVisualAmount();
        }
        else
        {
            IPopUpComponentData[] IAPFailedPopUpModules = new IPopUpComponentData[]
            {
                new HeaderPopUpComponentData(_localization.Localize("LOBBY_SHOP_IAPFAILED_HEADER"), true),
                new ImagePopUpComponentData("Skull"),
                new TextPopUpComponentData(_localization.Localize("LOBBY_SHOP_IAPFAILED_BODY")),
                new CloseButtonPopUpComponentData()
            };
            _popUps.SpawnPopUp(IAPFailedPopUpModules, transform.parent);
        }
    }

    private void UpdateInventoryVisualAmount()
    {
        _dilithium_Text.text = _gameProgress.CheckElement(ResourcesType.Dilithium).ToString();
        _allianceCredits_Text.text = _gameProgress.CheckElement(ResourcesType.AllianceCredits).ToString();
        _reputation_Text.text = _gameProgress.CheckElement(ResourcesType.Reputation).ToString();
        _fistAid_Text.text = _gameProgress.CheckElement(ResourcesType.FirstAidKit).ToString();
        _easyTrigger_Text.text = _gameProgress.CheckElement(ResourcesType.EasyTrigger).ToString();
        _deAthomizer_Text.text = _gameProgress.CheckElement(ResourcesType.DeAthomizer).ToString();
    }
    private void NotEnoughtCredits() 
    {
        IPopUpComponentData[] Modules = new IPopUpComponentData[]
        {
            new HeaderPopUpComponentData(_localization.Localize("LOBBY_MAIN_NOTENOUGHT"), true),
            new ImagePopUpComponentData("AllianceCredits"),
            new CloseButtonPopUpComponentData(),
        };
        _popUps.SpawnPopUp(Modules, transform.parent);
    }

    public async Task PurchaseGoldFromRewardedAd()
    {
        if(await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            _gameProgress.UpdateElement(ResourcesType.AllianceCredits, _gameConfig.AllianceCreditsPerRewardedAd);
            UpdateInventoryVisualAmount();
        }
    }
}
