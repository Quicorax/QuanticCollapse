using System;
using System.Collections.Generic;
using UnityEngine;

public class HangarShopView : MonoBehaviour
{
    [SerializeField] private StarshipVisuals _starshipVisuals;

    public RectTransform _colorPackParent;
    public RectTransform _geoParent;

    private DeSeializedStarshipColors _skinOnSight;
    private StarshipGeoModel _geoOnSight;
    private Action _transactionConfirmationOnSight;

    private GameProgressionService _gameProgression;
    private AddressablesService _addressables;
    private PopUpService _popUps;

    #region CachedPopUpModulesData
    private PopUpComponentData[] NotEnoughtCreitsModules = new PopUpComponentData[]
    {
            new HeaderPopUpComponentData(Constants.EmptyResource, true),
            new ImagePopUpComponentData(Constants.AllianceCredits),
            new CloseButtonPopUpComponentData(),
    };
    #endregion

    void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        _addressables = ServiceLocator.GetService<AddressablesService>();
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
            _addressables.SpawnAddressable<StarshipColorsView>(Constants.StarshipColor, _colorPackParent, x =>
            {
                x.InitStarshipColorView(colorPack.Value, !_gameProgression.CheckColorPackUnlockedByName(colorPack.Key), InteractWithColorPack);
            });

            _colorPackParent.sizeDelta += new Vector2(550, 0);
        }

        //StarshipGeo Buttons
        foreach (var starshipGeo in ServiceLocator.GetService<StarshipVisualsService>().StarshipGeo)
        {
            _addressables.SpawnAddressable<StarshipGeoView>(Constants.StarshipGeo, _geoParent, x => 
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

            PopUpComponentData[] Modules = new PopUpComponentData[]
            {
                new HeaderPopUpComponentData(starshipGeo.StarshipName, true),
                new TextPopUpComponentData(starshipGeo.StarshipDescription),
                new PricePopUpComponentData(starshipGeo.Price.ToString()),
                new ButtonPopUpComponentData(Constants.Buy, TryPurchaseProductGeo, true),
                new CloseButtonPopUpComponentData()
            };
            _popUps.SpawnPopUp(Modules, transform);
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

            PopUpComponentData[] Modules = new PopUpComponentData[]
            {
                new HeaderPopUpComponentData(colorPack.SkinName, true),
                new TextPopUpComponentData(colorPack.SkinDescription),
                new PricePopUpComponentData(colorPack.SkinPrice.ToString()),
                new ButtonPopUpComponentData(Constants.Buy, TryPurchaseProductColorPack, true),
                new CloseButtonPopUpComponentData()
            };
            _popUps.SpawnPopUp(Modules, transform);
        }
    }
    public void TryPurchaseProductGeo()
    {
        if (_gameProgression.CheckElement(Constants.AllianceCredits) >= _geoOnSight.Price)
        {
            _gameProgression.UnlockStarshipModel(_geoOnSight.StarshipName);
            _starshipVisuals.SetStarshipGeo(_geoOnSight.StarshipName);
            _transactionConfirmationOnSight?.Invoke();
        }
        else
            NotEnoughtCredits();

        _geoOnSight = null;
        _transactionConfirmationOnSight = null;
    }
    public void TryPurchaseProductColorPack()
    {
        if (_gameProgression.CheckElement(Constants.AllianceCredits) >= _skinOnSight.SkinPrice)
        {
            _gameProgression.UnlockColorPack(_skinOnSight.SkinName);
            _starshipVisuals.SetStarshipColors(_skinOnSight);
            _transactionConfirmationOnSight?.Invoke();
        }
        else
            NotEnoughtCredits();

        _skinOnSight = null;
        _transactionConfirmationOnSight = null;
    }

    void NotEnoughtCredits() => _popUps.SpawnPopUp(NotEnoughtCreitsModules, transform.parent);
}

