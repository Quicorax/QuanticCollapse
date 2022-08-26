using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class PlayModeTestsShop
{
    const string MasterScene = "00_MasterScene";
    const string Initial_Scene = "01_Initial_Scene";

    const string AlianceCredits = "AlianceCredits";

    [UnityTest]
    public IEnumerator TestCanBuyItem()
    {
        SceneManager.LoadScene(MasterScene);
        yield return new WaitForSecondsRealtime(1f);

        MasterSceneManager master = Object.FindObjectOfType<MasterSceneManager>();
        Assert.IsNotNull(master);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == Initial_Scene);

        GameObject.Find("Shop_Button").GetComponent<Button>().onClick.Invoke();
        yield return new WaitForSecondsRealtime(0.5f);

        ShopElementView elementView = GameObject.Find("ShopItems").GetComponentInChildren<ShopElementView>();
        Assert.NotNull(elementView);

        master.Inventory.AddElement(elementView.ElementModel.PriceKind, elementView.ElementModel.PriceAmount);

        int preShopElementAmount = master.Inventory.CheckElementAmount(elementView.ElementModel.ProductKind);

        elementView.gameObject.GetComponentInChildren<Button>().onClick.Invoke();

        int postShopElementAmount = master.Inventory.CheckElementAmount(elementView.ElementModel.ProductKind);

        master.Inventory.RemoveElement(elementView.ElementModel.ProductKind, elementView.ElementModel.ProductAmount);

        master.SaveAll();

        Assert.IsTrue(postShopElementAmount == (preShopElementAmount + elementView.ElementModel.ProductAmount));

    }
    [UnityTest]
    public IEnumerator TestCanNotBuyItem()
    {
        SceneManager.LoadScene(MasterScene);
        yield return new WaitForSecondsRealtime(1f);

        MasterSceneManager master = Object.FindObjectOfType<MasterSceneManager>();
        Assert.IsNotNull(master);

        master.Inventory.RemoveElement(AlianceCredits, 9999);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == Initial_Scene);

        GameObject.Find("Shop_Button").GetComponent<Button>().onClick.Invoke();
        yield return new WaitForSecondsRealtime(0.5f);

        ShopElementView elementView = GameObject.Find("ShopItems").GetComponentInChildren<ShopElementView>();
        Assert.NotNull(elementView);

        elementView.gameObject.GetComponentInChildren<Button>().onClick.Invoke();

        yield return new WaitForSecondsRealtime(0.2f);
        GameObject creditsPopUp = GameObject.Find("AlianceCreditsCap_PopUp");
        Assert.IsNotNull(creditsPopUp);

        Assert.IsTrue(creditsPopUp.activeSelf);
    }
}
