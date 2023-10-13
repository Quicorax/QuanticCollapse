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

        public List<AssetVersion> AssetVersions { get; private set; }

        public void Initialize(RemoteConfigGameService dataProvider)
        {
            InitialResources = dataProvider.GetFromJSON("InitialConfig_Resources", new List<ResourceElement>());
            InitialStarshipSkins = dataProvider.GetFromJSON("InitialConfig_StarshipSkins", new InitialStarshipSkins());

            ShopModel = dataProvider.GetFromJSON("Model_Shop", new List<ShopElementModel>());
            LevelsModel = dataProvider.GetFromJSON("Model_Levels", new List<LevelModel>());
            StarshipColorsModel = dataProvider.GetFromJSON("Model_StarshipColors", new List<StarshipColorsModel>());
            StarshipGeoModel = dataProvider.GetFromJSON("Model_StarshipGeo", new List<StarshipGeoModel>());
            GridBlocks = dataProvider.GetFromJSON("Model_GridBlocks", new GidBlocks());

            IAPProducts = dataProvider.GetFromJSON("Config_IAPProducts", new List<IAPBundle>());
            VideoAddRewards = dataProvider.GetFromJSON("Config_VideoAddRewards", new VideoAddRewards());
            AssetVersions = dataProvider.GetFromJSON("Config_AssetsVersions", new List<AssetVersion>());
        }

        public void Clear()
        {
        }
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

    [Serializable]
    public class AssetVersion
    {
        public string Key;
        public int Version;
    }
}