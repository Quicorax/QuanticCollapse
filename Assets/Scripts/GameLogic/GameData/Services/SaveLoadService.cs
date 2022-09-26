using UnityEngine;
using System.IO;
public class SaveLoadService : IService
{
    private static string kSavePath = Application.persistentDataPath + "/_gameProgression.json";

    [SerializeField] private GameProgressionService _gameProgression;
    [SerializeField] private GameConfigService _config;
    [SerializeField] private IGameProgressionProvider _gameProgressionProvider;

    public void Initialize(GameConfigService config, GameProgressionService gameProgression, IGameProgressionProvider gameProgressionProvider) 
    {
        _gameProgression = gameProgression;
        _config = config;
        _gameProgressionProvider = gameProgressionProvider;
        Load();
    }
    public void Save() => _gameProgressionProvider.Save(JsonUtility.ToJson(_gameProgression));

    private void Load()
    {
        string data = _gameProgressionProvider.Load();

        if (string.IsNullOrEmpty(data))
        {
            _gameProgression.LoadInitialResources(_config);
            Save();
        }
        else
            JsonUtility.FromJsonOverwrite(data, _gameProgression);
    }

    public void DeleteLocalFiles()
    {
        if (File.Exists(kSavePath)) 
            File.Delete(kSavePath);
    }
    public void Clear() { }
}