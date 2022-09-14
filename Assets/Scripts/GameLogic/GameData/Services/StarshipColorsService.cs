using System.Collections.Generic;
using UnityEngine;

public class StarshipColorsService : IService
{
    public Dictionary<string, DeSeializedStarshipColors> DeSerializedStarshipColors = new();
    public void Initialize()
    {
        ColorPackShopModel ColorPackShopModel = JsonUtility.FromJson<ColorPackShopModel>(Resources.Load<TextAsset>("StarshipColors").text);

        foreach (var colorPack in ColorPackShopModel.StarshipColors)
        {
            DeSerializedStarshipColors.Add(colorPack.SkinName, new(colorPack.SkinName, colorPack.SkinDescription,
                new Color().GenerateColorPackFromFormatedString(colorPack.ColorCode), colorPack.Price));
        }
    }

    public DeSeializedStarshipColors GetColorPackByName(string colorPackName)
    {
        DeSerializedStarshipColors.TryGetValue(colorPackName, out DeSeializedStarshipColors colorPack);
        return colorPack;
    }
    public void Clear() { }
}