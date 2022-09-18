using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressablesService : IService
{
    public async Task Initialize()
    {
        //Prewarm addressable assets
    }

    //SAMPLE CALL
    //var adrsInstance = await ServiceLocator.GetService<AddressablesService>()
    //        .SpawnAddressable<T>("Key", transform);
    //
    //adrsInstance.Init();

    public async Task<T> SpawnAddressable<T>(string key, Transform parent)
    {
        await Addressables.LoadAssetAsync<GameObject>(key).Task;

        GameObject loadedAsset = await Addressables.InstantiateAsync(key, parent).Task;

        return loadedAsset.GetComponent<T>();
    }

    public void ReleaseAddressable(GameObject addressableInstance) => Addressables.Release(addressableInstance);
    public void Clear() { }
}
