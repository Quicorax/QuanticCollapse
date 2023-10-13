using UnityEngine;
using System.IO;

namespace QuanticCollapse
{
    public class SaveLoadService : IService
    {
        private static string kSavePath = Application.persistentDataPath + "/_gameProgression.json";

        private GameProgressionService _gameProgression;
        private GameConfigService _config;
        private IGameProgressionProvider _gameProgressionProvider;

        public void Initialize(GameConfigService config, GameProgressionService gameProgression,
            IGameProgressionProvider gameProgressionProvider)
        {
            _gameProgression = gameProgression;
            _config = config;
            _gameProgressionProvider = gameProgressionProvider;
            Load();
        }

        public void Save() => _gameProgressionProvider.Save(JsonUtility.ToJson(_gameProgression));

        private void Load()
        {
            var data = _gameProgressionProvider.Load();

            if (string.IsNullOrEmpty(data))
            {
                _gameProgression.LoadInitialResources(_config);
            }
            else
            {
                JsonUtility.FromJsonOverwrite(data, _gameProgression);
            }
        }

        public void DeleteLocalFiles()
        {
            if (File.Exists(kSavePath))
            {
                File.Delete(kSavePath);
            }
        }

        public void Clear()
        {
        }
    }
}