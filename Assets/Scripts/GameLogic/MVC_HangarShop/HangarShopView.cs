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

    private MasterSceneManager _MasterSceneManager;
    private DeSeializedStarshipColors _skinOnSight;
    private StarshipGeoModel _geoOnSight;
    void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    void Start()
    {
        InitHangarShop();
    }
    void SetMasterReference(MasterSceneManager masterReference) => _MasterSceneManager = masterReference;
    void InitHangarShop()
    {
        HangarShopController = new();

        //Color Pack Buttons
        foreach (var colorPack in HangarShopController.DeSerializedStarshipColors)
        {
            Addressables.LoadAssetAsync<GameObject>(StarshipColorAdrsKey).Completed += handle =>
            {
                GameObject element = Addressables.InstantiateAsync(StarshipColorAdrsKey, _colorPackParent).Result;
                element.name = colorPack.SkinName;
                bool isLocked = _MasterSceneManager.Inventory.CheckSkinIsUnlockedByName(colorPack.SkinName);
                element.GetComponent<StarshipColorsView>().InitStarshipColorView(colorPack, isLocked, InteractWithSkinPack);

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
                bool isLocked = _MasterSceneManager.Inventory.CheckGeoIsUnlockedByName(starshipGeo.StarshipName);
                element.GetComponent<StarshipGeoView>().InitStarshipGeoView(starshipGeo, isLocked, InteractWithGeo);

                _geoParent.sizeDelta += new Vector2(100f, 0);
            };
        }
    }
    void InteractWithGeo(StarshipGeoModel geo, Action confirmation)
    {
        if (_MasterSceneManager.Inventory.CheckGeoIsUnlockedByName(geo.StarshipName))
        {
            _starshipVisuals.SetStarshipGeo(geo);
            confirmation?.Invoke();
        }
        else
        {
            _geoOnSight = geo;
            _transactionConfirmationOnSight = confirmation;

            List<PopUpComponentData> Modules = new()
            {
                new HeaderPopUpComponentData(geo.StarshipName, true),
                new TextPopUpComponentData(geo.StarshipDescription),
                new PricePopUpComponentData(geo.Price.ToString()),
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
    void InteractWithSkinPack(DeSeializedStarshipColors skin, Action confirmation)
    {
        if (_MasterSceneManager.Inventory.CheckSkinIsUnlockedByName(skin.SkinName))
        { 
            _starshipVisuals.SetStarshipColors(skin);
            confirmation?.Invoke();
        }
        else
        {
            _skinOnSight = skin;
            _transactionConfirmationOnSight = confirmation;

            List<PopUpComponentData> Modules = new()
            {
                new HeaderPopUpComponentData(skin.SkinName, true),
                new TextPopUpComponentData(skin.SkinDescription),
                new PricePopUpComponentData(skin.SkinPrice.ToString()),
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
        if (_MasterSceneManager.Inventory.CheckElementAmount(AlianceCredits) >= _geoOnSight.Price)
        {
            _MasterSceneManager.Inventory.AddElementToUnlockedGeo(_geoOnSight);
            _starshipVisuals.SetStarshipGeo(_geoOnSight);
            _transactionConfirmationOnSight?.Invoke();
            _MasterSceneManager.SaveAll();
        }
        else
            NotEnoughtCredits();

        _geoOnSight = null;
        _transactionConfirmationOnSight = null;
    }
    public void TryPurchaseProductColorPack()
    {
        if (_MasterSceneManager.Inventory.CheckElementAmount(AlianceCredits) >= _skinOnSight.SkinPrice)
        {
            _MasterSceneManager.Inventory.AddElementToUnlockedSkins(_skinOnSight);
            _starshipVisuals.SetStarshipColors(_skinOnSight);
            _transactionConfirmationOnSight?.Invoke();
            _MasterSceneManager.SaveAll();
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

