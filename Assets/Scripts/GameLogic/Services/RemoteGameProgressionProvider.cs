using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;

public class RemoteGameProgressionProvider : IGameProgressionProvider
{
    private string _remoteData = string.Empty;
    private bool _sendingToRemote;
    public async void SendSaveFiles()
    {
        _sendingToRemote = true; 
        await Task.Delay(500);

        try
        {
            await CloudSaveService.Instance.Data.ForceSaveAsync(
                new Dictionary<string, object> { { "data", _remoteData } });

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        _sendingToRemote = false;
        Debug.Log("Loaded  " + _remoteData + " for user " + AuthenticationService.Instance.PlayerId);
    }
    public async Task<bool> Initialize()
    {
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync();
       
        savedData.TryGetValue("data", out _remoteData);
        return true;
    }

    public string Load() => _remoteData;

    public void Save(string text)
    { 
        _remoteData = text;

        if(!_sendingToRemote)
            SendSaveFiles();
    }
}
