using System.Collections.Generic;
using UnityEngine;

namespace QuanticCollapse
{
    public class StarshipVisualsService : IService
    {
        public readonly Dictionary<string, DeSeializedStarshipColors> DeSerializedStarshipColors = new();

        private GameConfigService _config;

        public void Initialize()
        {
            _config = ServiceLocator.GetService<GameConfigService>();

            LoadColorPacks();
        }

        public DeSeializedStarshipColors GetColorPackByName(string colorPackName)
        {
            DeSerializedStarshipColors.TryGetValue(colorPackName, out var colorPack);
            return colorPack;
        }

        public void Clear()
        {
        }

        private void LoadColorPacks()
        {
            foreach (var colorPack in _config.StarshipColorsModel)
            {
                ColorUtility.TryParseHtmlString(colorPack.ColorCode[0], out var primaryColor);
                ColorUtility.TryParseHtmlString(colorPack.ColorCode[1], out var secondaryColor);
                ColorUtility.TryParseHtmlString(colorPack.ColorCode[2], out var signatureColor);

                DeSerializedStarshipColors.Add(colorPack.SkinName,
                    new(colorPack.SkinName,
                        colorPack.SkinDescription,
                        new[] { primaryColor, secondaryColor, signatureColor },
                        colorPack.Price));
            }
        }
    }
}