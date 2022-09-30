using System.Threading.Tasks;
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
    public void FocusLost() => _remote.FocusLost();
    public string Load()
    {
        string localData = _local.Load();
        string remoteData = _remote.Load();

        if (string.IsNullOrEmpty(localData) && string.IsNullOrEmpty(remoteData))
            return null;

        if (string.IsNullOrEmpty(localData) && !string.IsNullOrEmpty(remoteData))
            return remoteData;
        
        if (!string.IsNullOrEmpty(localData) && string.IsNullOrEmpty(remoteData))
            return localData;

        return CheckConflictingData(localData, remoteData);
    }

    string CheckConflictingData(string localData, string remoteData)
    {
        TicksDeSerializator localObject = JsonUtility.FromJson<TicksDeSerializator>(localData);
        TicksDeSerializator remoteObject = JsonUtility.FromJson<TicksDeSerializator>(remoteData);

        if (remoteObject._ticksPlayed > localObject._ticksPlayed)
            return remoteData;

        return localData;
    }

    public void Save(string text)
    {
        _local.Save(text);
        _remote.Save(text);
    }
}
[System.Serializable]
public class TicksDeSerializator
{
    public int _ticksPlayed;
} 
