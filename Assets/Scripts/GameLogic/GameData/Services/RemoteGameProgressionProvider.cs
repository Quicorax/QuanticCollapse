using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;

public class RemoteGameProgressionProvider : IGameProgressionProvider
{
    private string _remoteData = string.Empty;
    public RemoteGameProgressionProvider()
    {
        Application.focusChanged += OnApplicationFocusChanged;
    }
    private async void OnApplicationFocusChanged(bool hasFocus)
    {
        if (!hasFocus)
        {
            try
            {
                await CloudSaveService.Instance.Data.ForceSaveAsync(
                    new Dictionary<string, object> { { Constants.Data, _remoteData } });
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            Debug.Log("Loaded  " + _remoteData + " for user " + AuthenticationService.Instance.PlayerId);
        }
    }
    public async Task<bool> Initialize()
    {
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync();
       
        savedData.TryGetValue(Constants.Data, out _remoteData);
        Debug.Log("Loaded  " + _remoteData + " for user " + AuthenticationService.Instance.PlayerId);
        return true;
    }

    public string Load() => _remoteData;

    public void Save(string text) => _remoteData = text;
}
