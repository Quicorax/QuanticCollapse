using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

public class RemoteElementsTest
{
    const string shopURL = "https://script.google.com/macros/s/AKfycbyqWYKBcB31cnCl7YrjmJn6jlXZCPxiJTFIXZg9sM99ec322SdqhuuyVOQqqAW8iSyB4A/exec";
    const string levelsURL = "https://script.google.com/macros/s/AKfycbwTDEcnhXzTA9jgERQsMcI7QdR7JT9PGmuQMhiPao3jLgmcVFIbJgZMh-PRBglhsGAM/exec";
    const string starshipColorsURL = "https://script.google.com/macros/s/AKfycbzmoOarDMhtm5UWnxVqist_EEzwk9VsJf_9YP4Jup1F9WmHN8IcSilnsYSem9-aRozFvg/exec";
    const string starshipGeoURL = "https://script.google.com/macros/s/AKfycby-8doTKx5f5le1EMiB4RwV6Tc16bFQJhS_047ppqNBEXTkYjSK4ieGCUVHHHlLxqTE/exec";

    public void TestGetGenericReference<T>(out T genericObject) where T : Object
    {
        genericObject = Object.FindObjectOfType<T>();
        Assert.IsNotNull(genericObject);
    }


    [UnityTest]
    public IEnumerator LevelSelectorIsUpdated()
    {
        string localModel = Resources.Load<TextAsset>("Levels").text;
        string cloudModel = string.Empty;

        bool webRecuestCompleted = false;

        UnityWebRequest request = GameDataUpdater.WebRequest(levelsURL);
        request.SendWebRequest().completed += asyncOp =>
        {
            webRecuestCompleted = true;
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                return;
            }

            cloudModel = request.downloadHandler.text;
        };

        yield return new WaitUntil(()=> webRecuestCompleted);

        Assert.AreEqual(localModel, cloudModel);
    }

    [UnityTest]
    public IEnumerator ShopIsUpdated()
    {
        string localModel = Resources.Load<TextAsset>("ShopElements").text;
        string cloudModel = string.Empty;

        bool webRecuestCompleted = false;

        UnityWebRequest request = GameDataUpdater.WebRequest(shopURL);
        request.SendWebRequest().completed += asyncOp =>
        {
            webRecuestCompleted = true;
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                return;
            }

            cloudModel = request.downloadHandler.text;
        };

        yield return new WaitUntil(() => webRecuestCompleted);

        Assert.AreEqual(localModel, cloudModel);
    }

    [UnityTest]
    public IEnumerator StarshipColorsIsUpdated()
    {
        string localModel = Resources.Load<TextAsset>("StarshipColors").text;
        string cloudModel = string.Empty;

        bool webRecuestCompleted = false;

        UnityWebRequest request = GameDataUpdater.WebRequest(starshipColorsURL);
        request.SendWebRequest().completed += asyncOp =>
        {
            webRecuestCompleted = true;
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                return;
            }

            cloudModel = request.downloadHandler.text;
        };

        yield return new WaitUntil(() => webRecuestCompleted);

        Assert.AreEqual(localModel, cloudModel);
    }

    [UnityTest]
    public IEnumerator StarshipGeoIsUpdated()
    {
        string localModel = Resources.Load<TextAsset>("StarshipGeo").text;
        string cloudModel = string.Empty;

        bool webRecuestCompleted = false;

        UnityWebRequest request = GameDataUpdater.WebRequest(starshipGeoURL);
        request.SendWebRequest().completed += asyncOp =>
        {
            webRecuestCompleted = true;
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                return;
            }

            cloudModel = request.downloadHandler.text;
        };

        yield return new WaitUntil(() => webRecuestCompleted);

        Assert.AreEqual(localModel, cloudModel);
    }
}
