using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ShopView : MonoBehaviour
{
    const string Dilithium = "Dilithium";
    const string Reputation = "Reputation";
    const string AlianceCredits = "AlianceCredits";
    const string FirstAidKit = "FirstAidKit";
    const string EasyTrigger = "EasyTrigger";
    const string DeAthomizer = "DeAthomizer";

    const string ShopSectionAdrsKey = "ProductSection";

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;

    [SerializeField] private TMP_Text dilithium_Text;
    [SerializeField] private TMP_Text alianceCredits_Text;
    [SerializeField] private TMP_Text reputation_Text;
    [SerializeField] private TMP_Text fistAid_Text;
    [SerializeField] private TMP_Text easyTrigger_Text;
    [SerializeField] private TMP_Text deAthomizer_Text;

    private MasterSceneManager _MasterSceneManager;

    [SerializeField] private ShopController ShopController;
    [SerializeField] private RectTransform _parent;

    private List<string> productSectionAdded = new();

    void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    private void Start()
    {
        InitShop();
        UpdateInventoryVisualAmount();
    }
    void SetMasterReference(MasterSceneManager masterReference) => _MasterSceneManager = masterReference;
    public void InitShop()
    {
        ShopController = new(_MasterSceneManager);
        foreach (ShopElementModel shopElements in ShopController.ShopModel.ShopElements)
        {
            if (!productSectionAdded.Contains(shopElements.ProductKind))
            {
                productSectionAdded.Add(shopElements.ProductKind);

                Addressables.LoadAssetAsync<GameObject>(ShopSectionAdrsKey).Completed += handle =>
                {
                    ShopElementSection element = Addressables.InstantiateAsync(ShopSectionAdrsKey, _parent).Result.GetComponent<ShopElementSection>();
                    element.InitProductSection(shopElements.ProductKind, ShopController.ShopModel.ShopElements, TryPurchaseProduct, transform);
                    element.gameObject.name = ShopSectionAdrsKey + shopElements.ProductKind;

                    _parent.sizeDelta += new Vector2(0, 1000f);
                };
            }
        }
    }

    public void TryPurchaseProduct(ShopElementModel transactionData)
    {
        if (_MasterSceneManager.Inventory.CheckElementAmount(transactionData.PriceKind) >= transactionData.PriceAmount)
            ShopController.PurchaseElement(transactionData, UpdateInventoryVisualAmount);
        else
            NotEnoughtCredits();
    }
    void UpdateInventoryVisualAmount()
    {
        dilithium_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(Dilithium).ToString();
        alianceCredits_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(AlianceCredits).ToString();
        reputation_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(Reputation).ToString();

        fistAid_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(FirstAidKit).ToString();
        easyTrigger_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(EasyTrigger).ToString();
        deAthomizer_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(DeAthomizer).ToString();
    }

    void NotEnoughtCredits()
    {
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData("You don't have enought:", true),
            new ImagePopUpComponentData(AlianceCredits),
            new CloseButtonPopUpComponentData(),
        };

        Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
        {
            Addressables.InstantiateAsync("Modular_PopUp", transform.parent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }
}
