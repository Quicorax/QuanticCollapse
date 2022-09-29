using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LocalGameProgressionProvider : IGameProgressionProvider
{
    private static string _kSavePath = Application.persistentDataPath + "/_gameProgression.json";

    public async Task<bool> Initialize()
    {
        await Task.Yield();
        return true;
    }

    public string Load() => File.Exists(_kSavePath) ? File.ReadAllText(_kSavePath) : string.Empty;
    public void Save(string text) => File.WriteAllText(_kSavePath, text);
}