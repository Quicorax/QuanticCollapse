using System.Collections.Generic;
using UnityEngine;

namespace QuanticCollapse
{
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
                ColorUtility.TryParseHtmlString(colorPack.ColorCode[0], out Color primaryColor);
                ColorUtility.TryParseHtmlString(colorPack.ColorCode[1], out Color secondaryColor);
                ColorUtility.TryParseHtmlString(colorPack.ColorCode[2], out Color signatureColor);

                DeSerializedStarshipColors.Add(colorPack.SkinName,
                    new(colorPack.SkinName,
                        colorPack.SkinDescription,
                        new Color[] { primaryColor, secondaryColor, signatureColor },
                        colorPack.Price));
            }
        }
        public DeSeializedStarshipColors GetColorPackByName(string colorPackName)
        {
            DeSerializedStarshipColors.TryGetValue(colorPackName, out DeSeializedStarshipColors colorPack);
            return colorPack;
        }
        public void Clear() { }
    }
}