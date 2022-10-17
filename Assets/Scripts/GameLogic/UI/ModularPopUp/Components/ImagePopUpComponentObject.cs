using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace QuanticCollapse
{
    public class ImagePopUpComponentObject : MonoBehaviour, IPopUpComponentObject
    {
        public PopUpComponentType ModuleConcept;

        [SerializeField] private List<Sprite> sprites = new();

        [SerializeField] private Image ImageDisplay;
        [SerializeField] private GameObject ImageTextGameObject;
        [SerializeField] private TMP_Text ImageTextObject;

        private GameConfigService _config;
        private AddressablesService _addressables;

        public void Awake()
        {
            _config = ServiceLocator.GetService<GameConfigService>();
            _addressables = ServiceLocator.GetService<AddressablesService>();
        }
        public void SetData(IPopUpComponentData unTypedData, Action closeOnUse)
        {
            ImagePopUpComponentData data = unTypedData as ImagePopUpComponentData;

            ImageDisplay.sprite = GetSprite(data).Result;

            if (data.WithText)
            {
                ImageTextGameObject.SetActive(true);
                ImageTextObject.text = data.ImageText;
            }
        }

        private async Task<Sprite> GetSprite(ImagePopUpComponentData data)
        {
            Sprite asset;
            int version = _config.AssetVersions.Find(x => x.Key == data.SpriteName)?.Version ?? -1;

            if (version != -1)
                asset = await _addressables.LoadAssetVersion<Sprite>(data.SpriteName, version);
            else
                asset = sprites.Find(img => img.name == data.SpriteName);

            return asset;
        }
    }
}