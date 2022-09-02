using System.Collections;
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

    public ShopController ShopController;
    public PacksColorController ColorPackController;

    [SerializeField] private Transform _parent;

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
        InitProceduralShop();
        UpdateInventoryVisualAmount();
    }
    void SetMasterReference(MasterSceneManager masterReference) => _MasterSceneManager = masterReference;
    public void InitProceduralShop()
    {
        ShopController = new(_MasterSceneManager);
        ColorPackController = new();

        foreach (ShopElementModel shopElements in ShopController.ShopModel.ShopElements)
        {
            if (!productSectionAdded.Contains(shopElements.ProductKind))
            {
                productSectionAdded.Add(shopElements.ProductKind);

                Addressables.LoadAssetAsync<GameObject>(ShopSectionAdrsKey).Completed += handle =>
                {
                    ShopElementSection element = Addressables.InstantiateAsync(ShopSectionAdrsKey, _parent).Result.GetComponent<ShopElementSection>();
                    element.InitProductSection(shopElements.ProductKind, ShopController.ShopModel.ShopElements, TryPurchaseProduct);
                    element.gameObject.name = "ProductSection_" + shopElements.ProductKind;
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
        SpawnPopUp popUp = new SpawnPopUp(transform);
        popUp.SimpleGeneratePopUp(AlianceCredits);
    }
}
