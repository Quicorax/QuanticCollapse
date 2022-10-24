using System;
using TMPro;
using UnityEngine;


namespace QuanticCollapse
{
    public class HangarShopView : MonoBehaviour
    {
        [SerializeField] private StarshipVisuals _starshipVisuals;
        [SerializeField] private TMP_Text _allianceCredits_Text;

        public RectTransform _colorPackParent;
        public RectTransform _geoParent;

        private DeSeializedStarshipColors _skinOnSight;
        private StarshipGeoModel _geoOnSight;
        private Action _transactionConfirmationOnSight;

        private StarshipVisualsService _visualsStarship;
        private GameProgressionService _gameProgression;
        private LocalizationService _localization;
        private AddressablesService _addressables;
        private GameConfigService _config;
        private PopUpService _popUps;

        void Awake()
        {
            _visualsStarship = ServiceLocator.GetService<StarshipVisualsService>();
            _gameProgression = ServiceLocator.GetService<GameProgressionService>();
            _addressables = ServiceLocator.GetService<AddressablesService>();
            _localization = ServiceLocator.GetService<LocalizationService>();
            _config = ServiceLocator.GetService<GameConfigService>();
            _popUps = ServiceLocator.GetService<PopUpService>();

        }
        void Start()
        {
            InitHangarShop();
        }

        void InitHangarShop()
        {
            foreach (DeSeializedStarshipColors colorPack in _visualsStarship.DeSerializedStarshipColors.Values)
            {
                _addressables.LoadAdrsOfComponent<StarshipColorsView>("StarshipColorPack", _colorPackParent, h =>
                {
                    h.InitStarshipColorView(
                        colorPack,
                        !_gameProgression.CheckColorPackUnlockedByName(colorPack.SkinName),
                        InteractWithColorPack);
                });

                _colorPackParent.sizeDelta += new Vector2(550, 0);
            }

            foreach (StarshipGeoModel starshipGeo in _config.StarshipGeoModel)
            {
                _addressables.LoadAdrsOfComponent<StarshipGeoView>("StarshipGeo", _geoParent, h =>
                {
                    h.InitStarshipGeoView(
                        starshipGeo,
                        !_gameProgression.CheckStarshipUnlockedByName(starshipGeo.StarshipName),
                        InteractWithGeo);
                });

                _geoParent.sizeDelta += new Vector2(100f, 0);
            }
        }
        void InteractWithGeo(StarshipGeoModel starshipGeo, Action confirmation)
        {
            if (_gameProgression.CheckStarshipUnlockedByName(starshipGeo.StarshipName))
            {
                _starshipVisuals.SetStarshipGeo(starshipGeo.StarshipName);
                confirmation?.Invoke();
            }
            else
            {
                _geoOnSight = starshipGeo;
                _transactionConfirmationOnSight = confirmation;

                BuyGeoPopUp(starshipGeo);
            }
        }
        void InteractWithColorPack(DeSeializedStarshipColors colorPack, Action confirmation)
        {
            if (_gameProgression.CheckColorPackUnlockedByName(colorPack.SkinName))
            {
                _starshipVisuals.SetStarshipColors(colorPack);
                confirmation?.Invoke();
            }
            else
            {
                _skinOnSight = colorPack;
                _transactionConfirmationOnSight = confirmation;

                BuyColorPopUp(colorPack);
            }
        }
        public void TryPurchaseProductGeo()
        {
            if (_gameProgression.CheckElement("AllianceCredits") >= _geoOnSight.Price)
            {
                _gameProgression.UnlockStarshipModel(_geoOnSight.StarshipName, -_geoOnSight.Price);
                _allianceCredits_Text.text = _gameProgression.CheckElement("AllianceCredits").ToString();
                _starshipVisuals.SetStarshipGeo(_geoOnSight.StarshipName);
                _transactionConfirmationOnSight?.Invoke();
            }
            else
                NotEnoughtCredits("AllianceCredits");

            _geoOnSight = null;
            _transactionConfirmationOnSight = null;
        }
        public void TryPurchaseProductColorPack()
        {
            if (_gameProgression.CheckElement("AllianceCredits") >= _skinOnSight.SkinPrice)
            {
                _gameProgression.UnlockColorPack(_skinOnSight.SkinName, -_skinOnSight.SkinPrice);
                _allianceCredits_Text.text = _gameProgression.CheckElement("AllianceCredits").ToString();
                _starshipVisuals.SetStarshipColors(_skinOnSight);
                _transactionConfirmationOnSight?.Invoke();
            }
            else
                NotEnoughtCredits("AllianceCredits");

            _skinOnSight = null;
            _transactionConfirmationOnSight = null;
        }

        #region PopUps
        private void BuyGeoPopUp(StarshipGeoModel starshipGeo)
        {
            _popUps.SpawnPopUp(transform, new IPopUpComponentData[]
            {
                _popUps.AddHeader(starshipGeo.StarshipName, true),
                _popUps.AddText(_localization.Localize(starshipGeo.StarshipDescription)),
                _popUps.AddPrice(starshipGeo.Price.ToString()),
                _popUps.AddButton(_localization.Localize("LOBBY_MAIN_BUY"), TryPurchaseProductGeo, true),
                _popUps.AddCloseButton(),
            });
        }
        private void BuyColorPopUp(DeSeializedStarshipColors colorPack)
        {
            _popUps.SpawnPopUp(transform, new IPopUpComponentData[]
            {
                _popUps.AddHeader(colorPack.SkinName, true),
                _popUps.AddText(_localization.Localize(colorPack.SkinDescription)),
                _popUps.AddPrice(colorPack.SkinPrice.ToString()),
                _popUps.AddButton(_localization.Localize("LOBBY_MAIN_BUY"), TryPurchaseProductColorPack, true),
                _popUps.AddCloseButton(),
            });
        }
        private void NotEnoughtCredits(string resourceId)
        {
            _popUps.SpawnPopUp(transform.parent, new IPopUpComponentData[]
            {
                _popUps.AddHeader(_localization.Localize("LOBBY_MAIN_NOTENOUGHT"), true),
                _popUps.AddImage(resourceId, string.Empty),
                _popUps.AddCloseButton(),
            });
        }

        #endregion
    }
}