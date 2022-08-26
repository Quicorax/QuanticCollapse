using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;

public class ShopTests
{
    const string Initial_Scene_Path = "Assets/Scenes/01_Initial_Scene.unity";

    public bool TestGenericReference<T>(out T genericObject) where T : Object
    {
        genericObject = Object.FindObjectOfType<T>();
        return genericObject != null;
    }

    [Test]
    public void TestShopInitializes()
    {
        EditorSceneManager.OpenScene(Initial_Scene_Path);

        Assert.IsTrue(TestGenericReference<ShopView>(out ShopView view));
        view.Initialize();

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
