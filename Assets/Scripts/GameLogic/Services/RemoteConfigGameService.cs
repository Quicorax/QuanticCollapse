using System.Threading.Tasks;
using UnityEngine;
using System;
using Unity.Services.RemoteConfig;

namespace QuanticCollapse
{
    public class RemoteConfigGameService : IService
    {
        private struct appData
        {
        }

        private struct userData
        {
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T data;
        }

        private RuntimeConfig _config;

        public async Task Initialize() =>
            _config = await RemoteConfigService.Instance.FetchConfigsAsync(new userData(), new appData());

        public T GetFromJSON<T>(string key, T defaultValue = default)
        {
            var data = _config?.GetJson(key, "{}");

            if (string.IsNullOrEmpty(data))
            {
                return defaultValue;
            }

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

        public void Clear()
        {
        }
    }
}