using System;
using System.Collections.Generic;

namespace QuanticCollapse
{
    public class GameConfigService : IService
    {
        public List<ResourceElement> InitialResources { get; private set; }
        public InitialStarshipSkins InitialStarshipSkins { get; private set; }

        public List<ShopElementModel> ShopModel { get; private set; }
        public List<LevelModel> LevelsModel { get; private set; }

        public List<StarshipColorsModel> StarshipColorsModel { get; private set; }
        public List<StarshipGeoModel> StarshipGeoModel { get; private set; }
        public List<IAPBundle> IAPProducts { get; private set; }

        public VideoAddRewards VideoAddRewards { get; private set; }

        public GidBlocks GridBlocks { get; private set; }

        public void Initialize(RemoteConfigGameService dataProvider)
        {
            InitialResources = dataProvider.Get("InitialConfig_Resources", new List<ResourceElement>());
            InitialStarshipSkins = dataProvider.Get("InitialConfig_StarshipSkins", new InitialStarshipSkins());

            ShopModel = dataProvider.Get("Model_Shop", new List<ShopElementModel>());
            LevelsModel = dataProvider.Get("Model_Levels", new List<LevelModel>());
            StarshipColorsModel = dataProvider.Get("Model_StarshipColors", new List<StarshipColorsModel>());
            StarshipGeoModel = dataProvider.Get("Model_StarshipGeo", new List<StarshipGeoModel>());

            IAPProducts = dataProvider.Get("Config_IAPProducts", new List<IAPBundle>());
            VideoAddRewards = dataProvider.Get("Config_VideoAddRewards", new VideoAddRewards());

            GridBlocks = dataProvider.Get("Model_GridBlocks", new GidBlocks());
        }

        public void Clear() { }
    }

    [Serializable]
    public class InitialStarshipSkins
    {
        public List<string> Colors;
        public string Geo;
    }

    [Serializable]
    public class VideoAddRewards
    {
        public int AllianceCredits;
        public int ExternalBoosters;
    }
}