using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Purchasing;

namespace QuanticCollapse
{
    [System.Serializable]
    public class IAPBundle
    {
        public string ProductName;
        public TradeableItem Product;
        public string RemoteID;
    }

    public class IAPGameService : IIAPGameService, IStoreListener
    {
        private IStoreController _storeController = null;
        private TaskStatus _purchaseTask = TaskStatus.Created;

        private readonly Dictionary<string, string> _products = new();

        private bool _isInitialized;
        public bool IsReady() => _isInitialized;

        public void Initialize(GameConfigService config)
        {
            foreach (var item in config.IAPProducts)
            {
                _products.Add(item.ProductName, item.RemoteID);
            }

            _isInitialized = false;
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var productEntry in _products)
            {
                var ids = new IDs();
                ids.Add(productEntry.Value, new[] { GooglePlay.Name });
                builder.AddProduct(productEntry.Key, ProductType.Consumable, ids);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _isInitialized = true;
            _storeController = controller;
        }

        public void OnInitializeFailed(InitializationFailureReason error) => _isInitialized = false;
        public void OnInitializeFailed(InitializationFailureReason error, string message) => OnInitializeFailed(error);

        public async Task<bool> StartPurchase(string product)
        {
            if (!_isInitialized)
            {
                return false;
            }

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

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
            _purchaseTask = TaskStatus.Faulted;

        public string GetRemotePrice(string product)
        {
            if (!_isInitialized)
            {
                return string.Empty;
            }

            return _storeController.products.WithID(product)?.metadata?.localizedPriceString;
        }

        public void Clear()
        {
        }
    }
}