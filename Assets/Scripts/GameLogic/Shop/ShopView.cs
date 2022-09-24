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
    private GameConfigService _gameConfig;
    private IAPGameService _gameIAP;
    private PopUpService _popUps;

    private List<string> _productSectionAdded = new();
    private IAPBundle _iapBundleOnSight = null;


    #region CachedPopUpModulesData
    private IPopUpComponentData[] NotEnoughtCreditsPopUpModules = new IPopUpComponentData[]
    {
        new HeaderPopUpComponentData(Constants.EmptyResource, true),
        new ImagePopUpComponentData(Constants.AllianceCredits),
        new CloseButtonPopUpComponentData(),
    };

    IPopUpComponentData[] IAPFailedPopUpModules = new IPopUpComponentData[]
    {
        new HeaderPopUpComponentData(Constants.IAPFailed, true),
        new ImagePopUpComponentData(Constants.SkullIcon),
        new TextPopUpComponentData(Constants.IAPFailedLog),
        new CloseButtonPopUpComponentData()
    };
    #endregion

    private void Awake()
    {
        _gameProgress = ServiceLocator.GetService<GameProgressionService>();
        _addressables = ServiceLocator.GetService<AddressablesService>();
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

                _addressables.SpawnAddressable<ShopElementSection>(Constants.ShopSection, _parent, x => 
                { 
                    x.InitProductSection(shopElements.ProductKind, _shopController.ShopModel.ShopElements, TryPurchaseProduct, transform);
                    x.gameObject.name = Constants.ShopSection + shopElements.ProductKind;
                });
                
                _parent.sizeDelta += new Vector2(0, 1150f);
            }
        }
    }

    public void TryPurchaseProduct(ShopElementModel transactionData)
    {
        if (_gameProgress.AllianceCredits >= transactionData.PriceAmount)
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
            new ImagePopUpComponentData(Constants.AllianceCredits, Constants.X + _iapBundleOnSight.ProductAmount),
            new TextPopUpComponentData(_gameIAP.GetRemotePrice(productName)),
            new ButtonPopUpComponentData(Constants.Buy, TryPurchaseIAPProduct, true),
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
            _popUps.SpawnPopUp(IAPFailedPopUpModules, transform.parent);
    }

    private void UpdateInventoryVisualAmount()
    {
        _dilithium_Text.text = _gameProgress.Dilithium.ToString();
        _allianceCredits_Text.text = _gameProgress.AllianceCredits.ToString();
        _reputation_Text.text = _gameProgress.Reputation.ToString();
        _fistAid_Text.text = _gameProgress.FistAidKitBooster.ToString();
        _easyTrigger_Text.text = _gameProgress.EasyTriggerBooster.ToString();
        _deAthomizer_Text.text = _gameProgress.DeAthomizerBooster.ToString();
    }
    private void NotEnoughtCredits() => _popUps.SpawnPopUp(NotEnoughtCreditsPopUpModules, transform.parent);

    public async Task PurchaseGoldFromRewardedAd()
    {
        if(await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            _gameProgress.UpdateElement(ResourcesType.AllianceCredits, _gameConfig.AllianceCreditsPerRewardedAd);
            UpdateInventoryVisualAmount();
        }
    }
}
