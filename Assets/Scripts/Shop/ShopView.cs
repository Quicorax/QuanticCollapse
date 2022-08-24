using UnityEngine;

public class ShopView : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;

    [SerializeField] private ShopElementView _shopElementView;

    private MasterSceneManager _masterSceneManager;
    private ShopController _shopController;


    [SerializeField] private Transform _parent;

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
        _shopController.Init();
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

    void OnPurchaseElement(ShopElementModel elementModel) { _shopController.PurchaseElement(elementModel); }
}
