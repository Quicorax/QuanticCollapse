using System.Collections.Generic;
using UnityEngine;

public class HangarShopController
{
    public ColorPackShopModel HangarColorPackShopModel;
    public HangarStarshipGeoShopModel HangarStarshipGeoShopModel;

    public Dictionary<string, DeSeializedStarshipColors> DeSerializedStarshipColors = new();

    public HangarShopController()
    {
        LoadStarshipModelsList();
        DeSerializeColorModel();
    }

    void LoadStarshipModelsList()
    {
        HangarColorPackShopModel = JsonUtility.FromJson<ColorPackShopModel>(Resources.Load<TextAsset>("StarshipColors").text);
        HangarStarshipGeoShopModel = JsonUtility.FromJson<HangarStarshipGeoShopModel>(Resources.Load<TextAsset>("StarshipGeo").text);
    }
    void DeSerializeColorModel()
    {
        foreach (var colorPack in HangarColorPackShopModel.StarshipColors)
            DeSerializedStarshipColors.Add(colorPack.SkinName, new(colorPack.SkinName, colorPack.SkinDescription,  new Color().GenerateColorPackFromFormatedString(colorPack.ColorCode), colorPack.Price));
    }
}
