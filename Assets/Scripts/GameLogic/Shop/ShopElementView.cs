using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace QuanticCollapse
{
    public class ShopElementView : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites = new();
        [HideInInspector] public ShopElementModel TransactionData;

        private Action<ShopElementModel> _transaction;

        private GameConfigService _config;
        private AddressablesService _addressables;

        public void Awake()
        {
            _config = ServiceLocator.GetService<GameConfigService>();
            _addressables = ServiceLocator.GetService<AddressablesService>();
        }
        public async Task InitProduct(ShopElementModel transactionData, Action<ShopElementModel> transaction)
        {
            TransactionData = transactionData;

            Sprite asset;
            int version = _config.Versions.Find(x => x.Key == transactionData.ProductImage)?.Version ?? -1;

            if(version != -1)
                asset = await _addressables.LoadAssetVersion<Sprite>(transactionData.ProductImage, version);
            else
                asset = _sprites.Find(sprite => sprite.name == transactionData.ProductImage);

            GetComponent<Image>().sprite = asset;

            _transaction = transaction;
        }

        public void Purchase() => _transaction?.Invoke(TransactionData);
    }
}