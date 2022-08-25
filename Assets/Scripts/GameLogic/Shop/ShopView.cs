using UnityEngine;

public class ShopView : MonoBehaviour
{
    const string AlianceCredits = "AlianceCredits";

    [SerializeField] private GenericEventBus _ElementPurchased;
    [SerializeField] private GenericEventBus _NotEnoughtCredits;
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;

    [SerializeField] private ShopElementView _shopElementView;
    [SerializeField] private Transform _parent;

    private MasterSceneManager _masterSceneManager;
    private ShopController _shopController;


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
        _shopController = new(_masterSceneManager);
        Initialize();
    }
    void SetMasterReference(MasterSceneManager masterReference) { _masterSceneManager = masterReference; }

    public void Initialize()
    {
        foreach (ShopElementModel shopElements in _shopController.Model.ShopElements)
        {
            Instantiate(_shopElementView, _parent).InitElement(shopElements, OnPurchaseElement);
        }
    }

    void OnPurchaseElement(ShopElementModel elementModel) 
    {
        if (_masterSceneManager.Inventory.CheckElementAmount(AlianceCredits) >= elementModel.PriceAmount)
            _shopController.PurchaseElement(elementModel, _ElementPurchased);
        else
            _NotEnoughtCredits.NotifyEvent();
    }
}
