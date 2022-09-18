using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    private List<string> _productSectionAdded = new();

    private AddressablesService _addressables;

    #region CachedPopUpModulesData
    private PopUpComponentData[] NotEnoughtCreditsPopUpModules = new PopUpComponentData[]
    {
        new HeaderPopUpComponentData(Constants.EmptyResource, true),
        new ImagePopUpComponentData(Constants.AllianceCredits),
        new CloseButtonPopUpComponentData(),
    };
    #endregion

    private void Awake()
    {
        _addressables = ServiceLocator.GetService<AddressablesService>();
    }
    private void Start()
    {
        Initialize();
        UpdateInventoryVisualAmount();
    }
    public void Initialize()
    {
        _shopController = new();
        _gameProgress = ServiceLocator.GetService<GameProgressionService>();

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
                
                _parent.sizeDelta += new Vector2(0, 1000f);
            }
        }
    }

    public void TryPurchaseProduct(ShopElementModel transactionData)
    {
        if(_gameProgress.AllianceCredits >= transactionData.PriceAmount)
            _shopController.PurchaseElement(transactionData, UpdateInventoryVisualAmount);
        else
            NotEnoughtCredits();

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
    private void NotEnoughtCredits() => ServiceLocator.GetService<PopUpService>().SpawnPopUp(NotEnoughtCreditsPopUpModules, transform.parent);

    public async void PurchaseGoldFromRewardedAd()
    {
        if(await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            _gameProgress.UpdateElement(Constants.AllianceCredits, ServiceLocator.GetService<GameConfigService>().AllianceCreditsPerRewardedAd);
            UpdateInventoryVisualAmount();
        }
    }
}
