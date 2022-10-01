using System.Collections.Generic;
using UnityEngine;

public class StarshipVisualsService : IService
{
    public Dictionary<string, DeSeializedStarshipColors> DeSerializedStarshipColors = new();

    public GameConfigService _config;

    public void Initialize() 
    {
        _config = ServiceLocator.GetService<GameConfigService>();

        LoadColorPacks();
    }

    void LoadColorPacks()
    {
        foreach (var colorPack in _config.StarshipColorsModel)
        {
            DeSerializedStarshipColors.Add(colorPack.SkinName, new(colorPack.SkinName, colorPack.SkinDescription,
                new Color().GenerateColorsFromHexFormatedString(colorPack.ColorCode), colorPack.Price));
        }
    }
    public DeSeializedStarshipColors GetColorPackByName(string colorPackName)
    {
        DeSerializedStarshipColors.TryGetValue(colorPackName, out DeSeializedStarshipColors colorPack);
        return colorPack;
    }
    public void Clear() { }
}