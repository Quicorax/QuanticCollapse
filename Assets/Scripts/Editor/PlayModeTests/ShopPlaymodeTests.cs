using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class ShopPlaymodeTests
{
    const string MasterScene = "00_Master_Scene";
    const string Initial_Scene = "01_Initial_Scene";

    const string AlianceCredits = "AlianceCredits";

    public void TestGetGenericReference<T>(out T genericObject) where T : Object
    {
        genericObject = Object.FindObjectOfType<T>();
        Assert.IsNotNull(genericObject);
    }

    //[UnityTest]
    //public IEnumerator TestCanBuyItem()
    //{
    //    SceneManager.LoadScene(MasterScene);
    //    yield return new WaitForSecondsRealtime(1f);
    //
    //    TestGetGenericReference(out MasterSceneManager master);
    //
    //    Assert.AreEqual(SceneManager.GetSceneAt(1).name, Initial_Scene);
    //
    //    GameObject.Find("Shop_Button").GetComponent<Button>().onClick.Invoke();
    //    yield return new WaitForSecondsRealtime(0.5f);
    //
    //    TestGetGenericReference(out ShopElementView elementView);
    //    Assert.NotNull(elementView);
    //
    //    master.Inventory.AddElement(elementView.ElementModel.PriceKind, elementView.ElementModel.PriceAmount);
    //
    //    int preShopElementAmount = master.Inventory.CheckElementAmount(elementView.ElementModel.ProductName);
    //
    //    elementView.gameObject.GetComponentInChildren<Button>().onClick.Invoke();
    //
    //    int postShopElementAmount = master.Inventory.CheckElementAmount(elementView.ElementModel.ProductName);
    //
    //    master.Inventory.RemoveElement(elementView.ElementModel.ProductName, elementView.ElementModel.ProductAmount);
    //
    //    master.SaveAll();
    //
    //    Assert.AreEqual(postShopElementAmount, (preShopElementAmount + elementView.ElementModel.ProductAmount));
    //}
    //
    //[UnityTest]
    //public IEnumerator TestCanNotBuyItem()
    //{
    //    SceneManager.LoadScene(MasterScene);
    //    yield return new WaitForSecondsRealtime(1f);
    //
    //    TestGetGenericReference(out MasterSceneManager master);
    //
    //    master.Inventory.RemoveElement(AlianceCredits, 9999);
    //
    //    Assert.AreEqual(SceneManager.GetSceneAt(1).name, Initial_Scene);
    //
    //    GameObject.Find("Shop_Button").GetComponent<Button>().onClick.Invoke();
    //    yield return new WaitForSecondsRealtime(0.5f);
    //
    //    TestGetGenericReference(out ShopElementView elementView);
    //    Assert.NotNull(elementView);
    //
    //    elementView.gameObject.GetComponentInChildren<Button>().onClick.Invoke();
    //
    //    yield return new WaitForSecondsRealtime(0.2f);
    //    GameObject creditsPopUp = GameObject.Find("AlianceCreditsCap_PopUp");
    //    Assert.IsNotNull(creditsPopUp);
    //
    //    Assert.IsTrue(creditsPopUp.activeSelf);
    //}
}
