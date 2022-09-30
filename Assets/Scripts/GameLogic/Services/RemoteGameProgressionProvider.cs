using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;

public class RemoteGameProgressionProvider : IGameProgressionProvider
{
    private string _remoteData = string.Empty;
    public async void FocusLost()
    {
        try
        {
            await CloudSaveService.Instance.Data.ForceSaveAsync(
                new Dictionary<string, object> { { "data", _remoteData } });
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        Debug.Log("Loaded  " + _remoteData + " for user " + AuthenticationService.Instance.PlayerId);
    }
    public async Task<bool> Initialize()
    {
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync();
       
        savedData.TryGetValue("data", out _remoteData);
        Debug.Log("Loaded  " + _remoteData + " for user " + AuthenticationService.Instance.PlayerId);
        return true;
    }

    public string Load() => _remoteData;

    public void Save(string text)
    { 
        _remoteData = text;
    }
}
