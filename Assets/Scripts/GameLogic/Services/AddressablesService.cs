using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QuanticCollapse
{
    public class AddressablesService : IService
    {
        public void LoadAddrsOfComponent<T>(string key, Transform parent, Action<T> taskAction) =>
            LoadAddrsOfComponentAsync(key, parent, taskAction).ManageTaskException();

        private async Task LoadAddrsOfComponentAsync<T>(string key, Transform parent, Action<T> taskAction)
        {
            await Addressables.LoadAssetAsync<GameObject>(key).Task;
            var loadedAsset = await Addressables.InstantiateAsync(key, parent).Task;
            taskAction?.Invoke(loadedAsset.GetComponent<T>());
        }

        public async Task LoadAddrsPoolObject(string key, Action<GameObject> action) =>
            action?.Invoke(await Addressables.LoadAssetAsync<GameObject>(key).Task);

        public async Task<T> LoadAssetVersion<T>(string key, int version) =>
            await Addressables.LoadAssetAsync<T>($"{key}_v{version}").Task;

        public void ReleaseAddressable(GameObject addressableInstance) =>
            Addressables.Release(addressableInstance);

        public void Clear()
        {
        }
    }
}