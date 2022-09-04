using UnityEngine;
using UnityEngine.AddressableAssets;

public class HangarShopView : MonoBehaviour
{
    const string StarshipColorAdrsKey = "StarshipColorPack";
    const string AlianceCredits = "AlianceCredits";

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private StarshipVisuals _starshipVisuals;

    public Transform _parent;
    public HangarShopController HangarShopController;

    private MasterSceneManager _MasterSceneManager;
    private DeSeializedStarshipColors _skinOnSight;
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

        foreach (var colorPack in HangarShopController.DeSerializedStarshipColors)
        {
            Addressables.LoadAssetAsync<GameObject>(StarshipColorAdrsKey).Completed += handle =>
            {
                GameObject element = Addressables.InstantiateAsync(StarshipColorAdrsKey, _parent).Result;
                element.name = colorPack.SkinName;
                element.GetComponent<StarshipColorsView>().InitStarshipColorView(colorPack, InteractWithSkinPack);
            };
        }
    }
    void InteractWithSkinPack(DeSeializedStarshipColors skin)
    {
        if (_MasterSceneManager.Inventory.CheckSkinIsUnlockedByName(skin.SkinName))
            _starshipVisuals.SetStarshipColors(skin);
        else
        {
            _skinOnSight = skin;

            SpawnPopUp popUp = new SpawnPopUp(transform);

            PopUpData data = new();
            data.SetHeader(skin.SkinName, true);
            data.SetButton("Buy Product", TryPurchaseProduct);
            data.SetCloseButton();

            popUp.GeneratePopUp(data, false);
        }
    }
    public void TryPurchaseProduct()
    {
        if (_MasterSceneManager.Inventory.CheckElementAmount(AlianceCredits) >= _skinOnSight.SkinPrice)
        {
            _MasterSceneManager.Inventory.SaveFiles.Progres.UnlockedSkins.Add(_skinOnSight);
            _starshipVisuals.SetStarshipColors(_skinOnSight);

            _MasterSceneManager.SaveAll();
        }
        else
            NotEnoughtCredits();

        _skinOnSight = null;
    }
    void NotEnoughtCredits()
    {
        SpawnPopUp popUp = new SpawnPopUp(transform);
        popUp.SimpleGeneratePopUp(AlianceCredits);
    }
}
