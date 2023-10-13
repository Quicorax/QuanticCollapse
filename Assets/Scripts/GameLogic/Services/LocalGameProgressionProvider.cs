using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace QuanticCollapse
{
    public class LocalGameProgressionProvider : IGameProgressionProvider
    {
        private static string _savePath = Application.persistentDataPath + "/_gameProgression.json";

        public async Task<bool> Initialize()
        {
            await Task.Yield();
            return true;
        }

        public string Load() => File.Exists(_savePath) ? File.ReadAllText(_savePath) : string.Empty;
        public void Save(string text) => File.WriteAllText(_savePath, text);
    }
}