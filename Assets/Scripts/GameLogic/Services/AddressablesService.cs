using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QuanticCollapse
{
    public class AddressablesService : IService
    {
        public void SpawnAddressable<T>(string key, Transform parent, Action<T> taskAction)
            => AsyncSpawnAddressable(key, parent, taskAction).ManageTaskExeption();
        private async Task AsyncSpawnAddressable<T>(string key, Transform parent, Action<T> taskAction)
        {
            await Addressables.LoadAssetAsync<GameObject>(key).Task;

            GameObject loadedAsset = await Addressables.InstantiateAsync(key, parent).Task;

            taskAction?.Invoke(loadedAsset.GetComponent<T>());
        }

        public async Task SpawnAddressablePoolObject(string key, Action<GameObject> action)
            => action?.Invoke(await Addressables.LoadAssetAsync<GameObject>(key).Task);

        public async Task<T> LoadAssetVersion<T>(string key, int version) 
            => await Addressables.LoadAssetAsync<T>($"{key}_v{version}").Task;

        public void ReleaseAddressable(GameObject addressableInstance) => Addressables.Release(addressableInstance);
        public void Clear() { }
    }
}