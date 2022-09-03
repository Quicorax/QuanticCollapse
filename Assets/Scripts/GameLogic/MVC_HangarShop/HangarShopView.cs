using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HangarShopView : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private StarshipVisuals _starshipVisuals;

    const string StarshipColorAdrsKey = "StarshipColorPack";
    public Transform _parent;
    public HangarShopController HangarShopController;

    private MasterSceneManager _MasterSceneManager;
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
        if (_MasterSceneManager.SaveFiles.Progres.UnlockedSkins.Contains(skin))
        {
            Debug.Log("EQUIP");
            _starshipVisuals.SetStarshipColors(skin);
        }
        else
        {
            Debug.Log("BUY");
            _starshipVisuals.SetStarshipColors(skin);
        }
    }
}

public class HangarShopController
{
    public HangarShopModel HangarShopModel;
    public List<DeSeializedStarshipColors> DeSerializedStarshipColors = new();
    public HangarShopController()
    {
        LoadStarshipColorModelList();
        DeSerializeColorModel();
    }

    void LoadStarshipColorModelList() => HangarShopModel = JsonUtility.FromJson<HangarShopModel>(Resources.Load<TextAsset>("Colors").text);

    void DeSerializeColorModel()
    {
        foreach (var colorPack in HangarShopModel.StarshipColors)
            DeSerializedStarshipColors.Add(new(colorPack.SkinName, new Color().GenerateColorPackFromFormatedString(colorPack.ColorCode)));
    }
}

[System.Serializable]
public class HangarShopModel
{
    public List<StarshipColorsModel> StarshipColors = new();
}

[System.Serializable]
public class DeSeializedStarshipColors 
{
    public string SkinName;
    public Color[] SkinColors;

    public DeSeializedStarshipColors(string skinName, Color[] skinColors)
    {
        SkinName = skinName;
        SkinColors = skinColors;
    }
}
