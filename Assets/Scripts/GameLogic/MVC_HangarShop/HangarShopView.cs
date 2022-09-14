using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HangarShopView : MonoBehaviour
{
    const string StarshipColorAdrsKey = "StarshipColorPack";
    const string StarshipGeoAdrsKey = "StarshipGeo";

    const string AlianceCredits = "AlianceCredits";

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private StarshipVisuals _starshipVisuals;

    public RectTransform _colorPackParent;
    public RectTransform _geoParent;
    public HangarShopController HangarShopController;

    private MasterSceneTransitioner _MasterSceneManager;
    private DeSeializedStarshipColors _skinOnSight;
    private StarshipGeoModel _geoOnSight;

    private GameProgressionService _gameProgression;

    void Awake()
    {
        _MasterReference.Event += SetMasterReference;
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    void Start()
    {
        InitHangarShop();
    }
    void SetMasterReference(MasterSceneTransitioner masterReference) => _MasterSceneManager = masterReference;
    void InitHangarShop()
    {
        HangarShopController = new();

        //Color Pack Buttons
        foreach (var colorPack in HangarShopController.DeSerializedStarshipColors)
        {
            Addressables.LoadAssetAsync<GameObject>(StarshipColorAdrsKey).Completed += handle =>
            {
                GameObject element = Addressables.InstantiateAsync(StarshipColorAdrsKey, _colorPackParent).Result;
                element.name = colorPack.Key;
                bool isLocked = _gameProgression.CheckColorPackUnlockedByName(colorPack.Key);
                element.GetComponent<StarshipColorsView>().InitStarshipColorView(colorPack.Value, isLocked, InteractWithSkinPack);

                _colorPackParent.sizeDelta += new Vector2(550, 0);
            };
        }

        //StarshipGeo Buttons
        foreach (var starshipGeo in HangarShopController.HangarStarshipGeoShopModel.StarshipGeo)
        {
            Addressables.LoadAssetAsync<GameObject>(StarshipGeoAdrsKey).Completed += handle =>
            {
                GameObject element = Addressables.InstantiateAsync(StarshipGeoAdrsKey, _geoParent).Result;
                element.name = starshipGeo.StarshipName;
                bool isLocked = _gameProgression.CheckStarshipUnlockedByName(starshipGeo.StarshipName);
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
                new ButtonPopUpComponentData("Buy", TryPurchaseProductGeo, true),
                new CloseButtonPopUpComponentData()
            };

            Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
            {
                Addressables.InstantiateAsync("Modular_PopUp", transform)
                .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
            };
        }   
    }
    private Action _transactionConfirmationOnSight;
    void InteractWithSkinPack(DeSeializedStarshipColors colorPack, Action confirmation)
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
                new ButtonPopUpComponentData("Buy", TryPurchaseProductColorPack, true),
                new CloseButtonPopUpComponentData()
            };

            Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
            {
                Addressables.InstantiateAsync("Modular_PopUp", transform)
                .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
            };
        }
    }
    public void TryPurchaseProductGeo()
    {
        if (_gameProgression.CheckElement(AlianceCredits) >= _geoOnSight.Price)
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
        if (_gameProgression.CheckElement(AlianceCredits) >= _skinOnSight.SkinPrice)
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
            new HeaderPopUpComponentData("You don't have enought:", true),
            new ImagePopUpComponentData(AlianceCredits),
            new CloseButtonPopUpComponentData(),
        };

        Addressables.LoadAssetAsync<GameObject>("Modular_PopUp").Completed += handle =>
        {
            Addressables.InstantiateAsync("Modular_PopUp", transform.parent)
            .Result.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }
}

