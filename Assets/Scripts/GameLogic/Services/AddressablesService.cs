using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressablesService : IService
{
    public void SpawnAddressable<T>(string key, Transform parent, Action<T> taskAction)
    {
        CancellableSpawnAddressable(key, parent, taskAction).ManageTaskExeption();
    }

    private async Task CancellableSpawnAddressable<T>(string key, Transform parent, Action<T> taskAction)
    {
        await Addressables.LoadAssetAsync<GameObject>(key).Task;
        GameObject loadedAsset = await Addressables.InstantiateAsync(key, parent).Task;

        taskAction?.Invoke(loadedAsset.GetComponent<T>());
    }
    public async Task SpawnAddressableObject(string key, Transform parent, Action<GameObject> spawnedObject)
    {
        await Addressables.LoadAssetAsync<GameObject>(key).Task;
        GameObject loadedAsset = await Addressables.InstantiateAsync(key, parent).Task;

        spawnedObject?.Invoke(loadedAsset);
    }
    public void ReleaseAddressable(GameObject addressableInstance) => Addressables.Release(addressableInstance);
    public void Clear() { }
}