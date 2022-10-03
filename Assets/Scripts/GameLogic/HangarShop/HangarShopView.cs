using System;
using TMPro;
using UnityEngine;

public class HangarShopView : MonoBehaviour
{
    [SerializeField] private StarshipVisuals _starshipVisuals;
    [SerializeField] private TMP_Text _allianceCredits_Text;

    public RectTransform _colorPackParent;
    public RectTransform _geoParent;

    private DeSeializedStarshipColors _skinOnSight;
    private StarshipGeoModel _geoOnSight;
    private Action _transactionConfirmationOnSight;

    private GameProgressionService _gameProgression;
    private LocalizationService _localization;
    private AddressablesService _addressables;
    private GameConfigService _config;
    private PopUpService _popUps;

    void Awake()
    {
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
        //Color Pack Buttons
        foreach (var colorPack in ServiceLocator.GetService<StarshipVisualsService>().DeSerializedStarshipColors)
        {
            _addressables.SpawnAddressable<StarshipColorsView>("StarshipColorPack", _colorPackParent, x =>
            {
                x.InitStarshipColorView(colorPack.Value, !_gameProgression.CheckColorPackUnlockedByName(colorPack.Key), InteractWithColorPack);
            });

            _colorPackParent.sizeDelta += new Vector2(550, 0);
        }

        //StarshipGeo Buttons
        foreach (var starshipGeo in _config.StarshipGeoModel)
        {
            _addressables.SpawnAddressable<StarshipGeoView>("StarshipGeo", _geoParent, x => 
            {
                x.InitStarshipGeoView(starshipGeo, !_gameProgression.CheckStarshipUnlockedByName(starshipGeo.StarshipName), InteractWithGeo);
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

            _popUps.AddHeader(starshipGeo.StarshipName, true);
            _popUps.AddText(_localization.Localize(starshipGeo.StarshipDescription));
            _popUps.AddPrice(starshipGeo.Price.ToString());
            _popUps.AddButton(_localization.Localize("LOBBY_MAIN_BUY"), TryPurchaseProductGeo, true);
            _popUps.AddCloseButton();

            _popUps.SpawnPopUp(transform);
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

            _popUps.AddHeader(colorPack.SkinName, true);
            _popUps.AddText(_localization.Localize(colorPack.SkinDescription));
            _popUps.AddPrice(colorPack.SkinPrice.ToString());
            _popUps.AddButton(_localization.Localize("LOBBY_MAIN_BUY"), TryPurchaseProductColorPack, true);
            _popUps.AddCloseButton();

            _popUps.SpawnPopUp(transform);
        }
    }
    public void TryPurchaseProductGeo()
    {
        if (_gameProgression.CheckElement("AllianceCredits") >= _geoOnSight.Price) //TODO: Hard Codded!!!
        {
            _gameProgression.UnlockStarshipModel(_geoOnSight.StarshipName, -_geoOnSight.Price);
            _allianceCredits_Text.text = _gameProgression.CheckElement("AllianceCredits").ToString(); //TODO: Hard Codded!!!
            _starshipVisuals.SetStarshipGeo(_geoOnSight.StarshipName);
            _transactionConfirmationOnSight?.Invoke();
        }
        else
            NotEnoughtCredits("AllianceCredits"); //TODO: Hard Codded!!!

        _geoOnSight = null;
        _transactionConfirmationOnSight = null;
    }
    public void TryPurchaseProductColorPack()
    {
        if (_gameProgression.CheckElement("AllianceCredits") >= _skinOnSight.SkinPrice) //TODO: Hard Codded!!!
        {
            _gameProgression.UnlockColorPack(_skinOnSight.SkinName, -_skinOnSight.SkinPrice);
            _allianceCredits_Text.text = _gameProgression.CheckElement("AllianceCredits").ToString(); //TODO: Hard Codded!!!
            _starshipVisuals.SetStarshipColors(_skinOnSight);
            _transactionConfirmationOnSight?.Invoke();
        }
        else
            NotEnoughtCredits("AllianceCredits"); //TODO: Hard Codded!!!

        _skinOnSight = null;
        _transactionConfirmationOnSight = null;
    }

    void NotEnoughtCredits(string resourceId)
    {
        _popUps.AddHeader(_localization.Localize("LOBBY_MAIN_NOTENOUGHT"), true);
        _popUps.AddImage(resourceId, string.Empty); 
        _popUps.AddCloseButton();

        _popUps.SpawnPopUp(transform.parent);
    }
}

