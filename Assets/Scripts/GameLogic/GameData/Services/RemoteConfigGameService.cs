﻿using System.Threading.Tasks;
using UnityEngine;
using System;
using Unity.Services.RemoteConfig;

public class RemoteConfigGameService : IService
{
    private struct appData { }
    private struct userData { }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T data;
    }

    private RuntimeConfig _config;

    public async Task Initialize()
    {
        _config = await RemoteConfigService.Instance.FetchConfigsAsync(new userData(), new appData());

        switch (_config.origin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; Using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; Using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; Update values accordingly.");
                break;
        }
    }

    public string Get(string key, string defaultValue = "")
    {
        return _config?.GetString(key, defaultValue) ?? defaultValue;
    }

    public int Get(string key, int defaultValue = 0)
    {
        return _config?.GetInt(key, defaultValue) ?? defaultValue;
    }

    public float Get(string key, float defaultValue = 0)
    {
        return _config?.GetFloat(key, defaultValue) ?? defaultValue;
    }

    public bool Get(string key, bool defaultValue = false)
    {
        return _config?.GetBool(key, defaultValue) ?? defaultValue;
    }

    public T Get<T>(string key, T defaultValue = default)
    {
        string data = _config?.GetString(key, "{}");
        if (string.IsNullOrEmpty(data))
            return defaultValue;

        try
        {
            return JsonUtility.FromJson<Wrapper<T>>(data).data;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return defaultValue;
        }
    }

    public void Clear() { }
}