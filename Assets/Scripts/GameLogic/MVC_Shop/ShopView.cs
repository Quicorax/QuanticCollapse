using UnityEngine;
using UnityEngine.AddressableAssets;

public class ShopView : MonoBehaviour
{
    const string AlianceCredits = "AlianceCredits";

    const string ShopAdrsKey = "ShopElement_ViewObject";

    [SerializeField] private GenericEventBus _ElementPurchased;
    [SerializeField] private GenericEventBus _NotEnoughtCredits;
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    private MasterSceneManager _masterSceneManager;

    [SerializeField] private Transform _parent;

    public ShopController ShopController;


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
        Initialize();
    }

    void SetMasterReference(MasterSceneManager masterReference) => _masterSceneManager = masterReference;
    public void Initialize()
    {
        ShopController = new(_masterSceneManager);

        foreach (ShopElementModel shopElements in ShopController.ShopModel.ShopElements)
        {
            Addressables.LoadAssetAsync<GameObject>(ShopAdrsKey).Completed += handle =>
            {
                GameObject element = Addressables.InstantiateAsync(ShopAdrsKey, _parent).Result;
                element.GetComponent<ShopElementView>().InitElement(shopElements, OnPurchaseElement);
            };
        }
    }

    void OnPurchaseElement(ShopElementModel elementModel) 
    {
        //if (_masterSceneManager.Inventory.CheckElementAmount(AlianceCredits) >= elementModel.PriceAmount)
        //    ShopController.PurchaseElement(elementModel, _ElementPurchased);
        //else
        //    _NotEnoughtCredits.NotifyEvent();
    }
}
