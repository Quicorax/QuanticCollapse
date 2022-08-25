using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TestsShop
{
    [Test]
    public void TestShopInitializes()
    {
        ShopView view = Object.FindObjectOfType<ShopView>();
        view.Initialize();
        Assert.NotNull(view.Controller.Model);

        Transform shopItemsParent = GameObject.Find("ShopItems").transform;
        Assert.NotNull(shopItemsParent);

        Assert.IsTrue(shopItemsParent.childCount == view.Controller.Model.ShopElements.Count);
    }

    [UnityTest]
    public IEnumerator TestShopIsUpdated()
    {
        string localShopModel = Resources.Load<TextAsset>("ShopElements").text;
        string cloudShopModel = string.Empty;
        bool webRecuestCompleted = false;

        UnityWebRequest request = GameDataUpdater.WebRequest();
        request.SendWebRequest().completed += asyncOp =>
        {
            webRecuestCompleted = true;
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                return;
            }

            cloudShopModel = request.downloadHandler.text;
        };

        yield return new WaitUntil(()=> webRecuestCompleted);

        Assert.IsTrue(localShopModel == cloudShopModel);
    }
}
