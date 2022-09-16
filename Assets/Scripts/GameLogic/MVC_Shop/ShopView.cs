using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ShopView : MonoBehaviour
{
    const string ShopSectionAdrsKey = "ProductSection";

    [SerializeField] private TMP_Text dilithium_Text;
    [SerializeField] private TMP_Text alianceCredits_Text;
    [SerializeField] private TMP_Text reputation_Text;
    [SerializeField] private TMP_Text fistAid_Text;
    [SerializeField] private TMP_Text easyTrigger_Text;
    [SerializeField] private TMP_Text deAthomizer_Text;

    [SerializeField] private ShopController _shopController;
    [SerializeField] private RectTransform _parent;

    private GameProgressionService _gameProgress;

    private List<string> productSectionAdded = new();

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
            if (!productSectionAdded.Contains(shopElements.ProductKind))
            {
                productSectionAdded.Add(shopElements.ProductKind);

                Addressables.LoadAssetAsync<GameObject>(ShopSectionAdrsKey).Completed += handle =>
                {
                    ShopElementSection element = Addressables.InstantiateAsync(ShopSectionAdrsKey, _parent).Result.GetComponent<ShopElementSection>();
                    element.InitProductSection(shopElements.ProductKind, _shopController.ShopModel.ShopElements, TryPurchaseProduct, transform);
                    element.gameObject.name = ShopSectionAdrsKey + shopElements.ProductKind;

                    _parent.sizeDelta += new Vector2(0, 1000f);
                };
            }
        }
    }

    public void TryPurchaseProduct(ShopElementModel transactionData)
    {
        if(_gameProgress.AlianceCredits >= transactionData.PriceAmount)
            _shopController.PurchaseElement(transactionData, UpdateInventoryVisualAmount);
        else
            NotEnoughtCredits();

    }
    void UpdateInventoryVisualAmount()
    {
        dilithium_Text.text = _gameProgress.Dilithium.ToString();
        alianceCredits_Text.text = _gameProgress.AlianceCredits.ToString();
        reputation_Text.text = _gameProgress.Reputation.ToString();
        fistAid_Text.text = _gameProgress.FistAidKitBooster.ToString();
        easyTrigger_Text.text = _gameProgress.EasyTriggerBooster.ToString();
        deAthomizer_Text.text = _gameProgress.DeAthomizerBooster.ToString();
    }
    void NotEnoughtCredits()
    {
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData("You don't have enought:", true),
            new ImagePopUpComponentData("AlianceCredits"),
            new CloseButtonPopUpComponentData(),
        };

        Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
        {
            Addressables.InstantiateAsync("Modular_PopUp", transform.parent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }

    public async void PurchaseGoldFromRewardedAd()
    {
        if(await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            _gameProgress.UpdateElement("AlianceCredits", 10);
            UpdateInventoryVisualAmount();
        }
    }
}
