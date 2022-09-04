using System;
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

                _colorPackParent.sizeDelta += new Vector2(300f, 0);
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
            _starshipVisuals.SetStarshipGeo(geo);
        else
        {
            _geoOnSight = geo;
            _transactionConfirmationOnSight = confirmation;
            SpawnPopUp popUp = new SpawnPopUp(transform);
            
            PopUpData data = new();
            data.SetHeader(geo.StarshipName, true);
            data.SetBodyText(geo.StarshipDescription);
            data.SetButton("Buy Product", TryPurchaseProductGeo);
            data.SetCloseButton();
            
            popUp.GeneratePopUp(data, false);
        }   
    }
    private Action _transactionConfirmationOnSight;
    void InteractWithSkinPack(DeSeializedStarshipColors skin, Action confirmation)
    {
        if (_MasterSceneManager.Inventory.CheckSkinIsUnlockedByName(skin.SkinName))
            _starshipVisuals.SetStarshipColors(skin);
        else
        {
            _skinOnSight = skin;
            _transactionConfirmationOnSight = confirmation;

            SpawnPopUp popUp = new SpawnPopUp(transform);

            PopUpData data = new();
            data.SetHeader(skin.SkinName, true);
            data.SetBodyText(skin.SkinDescription);
            data.SetButton("Buy Product", TryPurchaseProductColorPack);
            data.SetCloseButton();

            popUp.GeneratePopUp(data, false);
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
        SpawnPopUp popUp = new SpawnPopUp(transform);
        popUp.SimpleGeneratePopUp(AlianceCredits);
    }
}

