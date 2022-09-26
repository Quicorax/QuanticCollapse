using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Purchasing;

[System.Serializable]
public class IAPBundle
{
    public string ProductName;
    public int ProductAmount;
}
public class IAPGameService : IIAPGameService, IStoreListener
{
    private bool _isInitialized = false;

    private IStoreController _storeController = null;

    private TaskStatus _purchaseTask = TaskStatus.Created;
    private TaskStatus _initializeTask = TaskStatus.Created;

    private Dictionary<string, string> _products;
    public bool IsReady() => _isInitialized;

    public async Task Initialize()
    {
        _products = new()
        {
            ["AllianceCredits Pack 0"] = "com.quicorax.quanticcollapse_ac0",
            ["AllianceCredits Pack 1"] = "com.quicorax.quanticcollapse_ac1",
            ["AllianceCredits Pack 2"] = "com.quicorax.quanticcollapse_ac2"
        };

        _isInitialized = false;
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (KeyValuePair<string, string> productEntry in _products)
        {
            var ids = new IDs();
            ids.Add(productEntry.Value, new[] { GooglePlay.Name });
            builder.AddProduct(productEntry.Key, ProductType.Consumable, ids);
        }
        _initializeTask = TaskStatus.Running;
        UnityPurchasing.Initialize(this, builder);

        while(_initializeTask == TaskStatus.Running)
            await Task.Delay(100);

    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _isInitialized = true;
        _initializeTask = TaskStatus.RanToCompletion;
        _storeController = controller;
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        _isInitialized = false;
        _initializeTask = TaskStatus.Faulted;
    }

    public async Task<bool> StartPurchase(string product)
    {
        if (!_isInitialized)
            return false;

        _purchaseTask = TaskStatus.Running;
        _storeController.InitiatePurchase(product);

        while (_purchaseTask == TaskStatus.Running)
        {
            await Task.Delay(500);
        }

        return _purchaseTask == TaskStatus.RanToCompletion;
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        _purchaseTask = TaskStatus.RanToCompletion;
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) => _purchaseTask = TaskStatus.Faulted;

    public string GetRemotePrice(string product)
    {
        if (!_isInitialized)
            return string.Empty;

        Product unityProduct = _storeController.products.WithID(product);
        return unityProduct?.metadata?.localizedPriceString;
    }
    public void Clear() { }
}
