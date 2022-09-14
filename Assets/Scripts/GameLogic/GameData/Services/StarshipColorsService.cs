using System.Collections.Generic;
using UnityEngine;

public class StarshipColorsService : IService
{
    private Dictionary<string, DeSeializedStarshipColors> _deSerializedStarshipColors = new();
    public void Initialize()
    {
        ColorPackShopModel ColorPackShopModel = JsonUtility.FromJson<ColorPackShopModel>(Resources.Load<TextAsset>("StarshipColors").text);

        foreach (var colorPack in ColorPackShopModel.StarshipColors)
        {
            _deSerializedStarshipColors.Add(colorPack.SkinName, new(colorPack.SkinName, colorPack.SkinDescription,
                new Color().GenerateColorPackFromFormatedString(colorPack.ColorCode), colorPack.Price));
        }
    }

    public DeSeializedStarshipColors GetColorPackByName(string colorPackName)
    {
        _deSerializedStarshipColors.TryGetValue(colorPackName, out DeSeializedStarshipColors colorPack);
        return colorPack;
    }

    public void Clear() { }
}