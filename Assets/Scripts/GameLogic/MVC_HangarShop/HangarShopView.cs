using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HangarShopView : MonoBehaviour
{
    [SerializeField] private StarshipVisuals _starshipVisuals;

    public RectTransform _colorPackParent;
    public RectTransform _geoParent;

    private DeSeializedStarshipColors _skinOnSight;
    private StarshipGeoModel _geoOnSight;
    private Action _transactionConfirmationOnSight;

    private GameProgressionService _gameProgression;

    void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
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
            Addressables.LoadAssetAsync<GameObject>(Constants.StarshipColor).Completed += handle =>
            {
                GameObject element = Addressables.InstantiateAsync(Constants.StarshipColor, _colorPackParent).Result;
                element.name = colorPack.Key;
                bool isLocked = !_gameProgression.CheckColorPackUnlockedByName(colorPack.Key);
                element.GetComponent<StarshipColorsView>().InitStarshipColorView(colorPack.Value, isLocked, InteractWithColorPack);

                _colorPackParent.sizeDelta += new Vector2(550, 0);
            };
        }

        //StarshipGeo Buttons
        foreach (var starshipGeo in ServiceLocator.GetService<StarshipVisualsService>().StarshipGeo)
        {
            Addressables.LoadAssetAsync<GameObject>(Constants.StarshipGeo).Completed += handle =>
            {
                GameObject element = Addressables.InstantiateAsync(Constants.StarshipGeo, _geoParent).Result;
                element.name = starshipGeo.StarshipName;
                bool isLocked = !_gameProgression.CheckStarshipUnlockedByName(starshipGeo.StarshipName);
                element.GetComponent<StarshipGeoView>().InitStarshipGeoView(starshipGeo, isLocked, InteractWithGeo);

                _geoParent.sizeDelta += new Vector2(100f, 0);
            };
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

            List<PopUpComponentData> Modules = new()
            {
                new HeaderPopUpComponentData(starshipGeo.StarshipName, true),
                new TextPopUpComponentData(starshipGeo.StarshipDescription),
                new PricePopUpComponentData(starshipGeo.Price.ToString()),
                new ButtonPopUpComponentData(Constants.Buy, TryPurchaseProductGeo, true),
                new CloseButtonPopUpComponentData()
            };

            Addressables.LoadAssetAsync<GameObject>(Constants.ModularPopUp).Completed += handle =>
            {
                Addressables.InstantiateAsync(Constants.ModularPopUp, transform)
                .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
            };
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

            List<PopUpComponentData> Modules = new()
            {
                new HeaderPopUpComponentData(colorPack.SkinName, true),
                new TextPopUpComponentData(colorPack.SkinDescription),
                new PricePopUpComponentData(colorPack.SkinPrice.ToString()),
                new ButtonPopUpComponentData(Constants.Buy, TryPurchaseProductColorPack, true),
                new CloseButtonPopUpComponentData()
            };

            Addressables.LoadAssetAsync<GameObject>(Constants.ModularPopUp).Completed += handle =>
            {
                Addressables.InstantiateAsync(Constants.ModularPopUp, transform)
                .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
            };
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
    void NotEnoughtCredits()
    {
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData(Constants.EmptyResource, true),
            new ImagePopUpComponentData(Constants.AllianceCredits),
            new CloseButtonPopUpComponentData(),
        };

        Addressables.LoadAssetAsync<GameObject>(Constants.ModularPopUp).Completed += handle =>
        {
            Addressables.InstantiateAsync(Constants.ModularPopUp, transform.parent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }
}

