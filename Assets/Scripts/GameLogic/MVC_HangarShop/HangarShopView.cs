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

    void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        _addressables = ServiceLocator.GetService<AddressablesService>();
    }
    void Start()
    {
        InitHangarShop();
    }
    async void InitHangarShop()
    {
        //Color Pack Buttons
        foreach (var colorPack in ServiceLocator.GetService<StarshipVisualsService>().DeSerializedStarshipColors)
        {
            var adrsInstance = await _addressables
                .SpawnAddressable<StarshipColorsView>(Constants.StarshipColor, _colorPackParent);

            adrsInstance.InitStarshipColorView(colorPack.Value, 
                !_gameProgression.CheckColorPackUnlockedByName(colorPack.Key), 
                InteractWithColorPack);

            _colorPackParent.sizeDelta += new Vector2(550, 0);
        }

        //StarshipGeo Buttons
        foreach (var starshipGeo in ServiceLocator.GetService<StarshipVisualsService>().StarshipGeo)
        {
            var adrsInstance = await _addressables
                .SpawnAddressable<StarshipGeoView>(Constants.StarshipGeo, _geoParent);

            adrsInstance.InitStarshipGeoView(starshipGeo,
                !_gameProgression.CheckStarshipUnlockedByName(starshipGeo.StarshipName),
                InteractWithGeo);

            _geoParent.sizeDelta += new Vector2(100f, 0);
        }
    }
    async void InteractWithGeo(StarshipGeoModel starshipGeo, Action confirmation)
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

            List<PopUpComponentData> Modules = new()
            {
                new HeaderPopUpComponentData(starshipGeo.StarshipName, true),
                new TextPopUpComponentData(starshipGeo.StarshipDescription),
                new PricePopUpComponentData(starshipGeo.Price.ToString()),
                new ButtonPopUpComponentData(Constants.Buy, TryPurchaseProductGeo, true),
                new CloseButtonPopUpComponentData()
            };

            var adrsInstance = await ServiceLocator.GetService<AddressablesService>()
                .SpawnAddressable<ModularPopUp>(Constants.ModularPopUp, transform);

            adrsInstance.GeneratePopUp(Modules);
        }   
    }
    async void InteractWithColorPack(DeSeializedStarshipColors colorPack, Action confirmation)
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

            List<PopUpComponentData> Modules = new()
            {
                new HeaderPopUpComponentData(colorPack.SkinName, true),
                new TextPopUpComponentData(colorPack.SkinDescription),
                new PricePopUpComponentData(colorPack.SkinPrice.ToString()),
                new ButtonPopUpComponentData(Constants.Buy, TryPurchaseProductColorPack, true),
                new CloseButtonPopUpComponentData()
            };

            var adrsInstance = await ServiceLocator.GetService<AddressablesService>()
                .SpawnAddressable<ModularPopUp>(Constants.ModularPopUp, transform);

            adrsInstance.GeneratePopUp(Modules);
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
    async void NotEnoughtCredits()
    {
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData(Constants.EmptyResource, true),
            new ImagePopUpComponentData(Constants.AllianceCredits),
            new CloseButtonPopUpComponentData(),
        };

        var adrsInstance = await ServiceLocator.GetService<AddressablesService>()
            .SpawnAddressable<ModularPopUp>(Constants.ModularPopUp, transform.parent);

        adrsInstance.GeneratePopUp(Modules);
    }
}

