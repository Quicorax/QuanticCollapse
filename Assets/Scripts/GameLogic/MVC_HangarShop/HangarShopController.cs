using System.Collections.Generic;
using UnityEngine;

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
            DeSerializedStarshipColors.Add(new(colorPack.SkinName, new Color().GenerateColorPackFromFormatedString(colorPack.ColorCode), colorPack.Price));
    }
}
