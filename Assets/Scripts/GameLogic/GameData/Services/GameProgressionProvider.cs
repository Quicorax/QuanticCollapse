using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameProgressionProvider : IGameProgressionProvider
{
    private LocalGameProgressionProvider _local = new LocalGameProgressionProvider();
    private RemoteGameProgressionProvider _remote = new RemoteGameProgressionProvider();

    public async Task<bool> Initialize()
    {
        await Task.WhenAll(_local.Initialize(), _remote.Initialize());
        return true;
    }

    public string Load()
    {
        string localData = _local.Load();
        string remoteData = _remote.Load();

        if (string.IsNullOrEmpty(localData) && !string.IsNullOrEmpty(remoteData))
        {
            return remoteData;
        }

        if (!string.IsNullOrEmpty(localData) && string.IsNullOrEmpty(remoteData))
        {
            return localData;
        }

        string[] remote = remoteData.Split(",");
        string[] remoteGameTicks = remote[0].Split(":");
        int remoteTicks = int.Parse(remoteGameTicks[1]);

        string[] local = localData.Split(",");
        string[] localGameTicks = local[0].Split(":");
        int localTicks = int.Parse(localGameTicks[1]);

        if (remoteTicks > localTicks)
        {
            return remoteData;
        }

        return localData;
    }

    public void Save(string text)
    {
        _local.Save(text);
        _remote.Save(text);
    }
}
