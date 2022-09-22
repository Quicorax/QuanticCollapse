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

        return CheckConflictingData(localData, remoteData);
    }

    string CheckConflictingData(string localData, string remoteData)
    {
        int remoteTicks;
        int localTicks;

        #region Get dominant data by splitting raw data string and compare int parsed value
        //string[] remote = remoteData.Split(",");
        //string[] remoteGameTicks = remote[0].Split(":");
        //remoteTicks = int.Parse(remoteGameTicks[1]);
        //
        //string[] local = localData.Split(",");
        //string[] localGameTicks = local[0].Split(":");
        //localTicks = int.Parse(localGameTicks[1]);
        #endregion

        #region Get dominant data by deserializing both raw data and comparing accesible value
        var localObject = new MockGameProgressionService();
        JsonUtility.FromJsonOverwrite(localData, localObject);
        localTicks = localObject.TicksPlayed;

        var remoteObject = new MockGameProgressionService();
        JsonUtility.FromJsonOverwrite(remoteData, remoteObject);
        remoteTicks = remoteObject.TicksPlayed;
        #endregion

        Debug.Log("Local ticks: " + localTicks + " VS " + "Remote ticks: " + remoteTicks);

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

public class MockGameProgressionService : GameProgressionService
{

}
