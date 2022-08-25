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
        yield return new WaitForSecondsRealtime(3f);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == Initial_Scene);

        MasterSceneManager master = Object.FindObjectOfType<MasterSceneManager>();
        Assert.IsNotNull(master);

        master.Inventory.AddElement(AlianceCredits, 999);

        ShopView view = Object.FindObjectOfType<ShopView>();
        view.Initialize();
        Assert.NotNull(view);

        Transform shopItemsParent = GameObject.Find("ShopItems").transform;
        Assert.NotNull(shopItemsParent);

        yield return new WaitForSecondsRealtime(0.5f);

        ShopElementView elementView = shopItemsParent.GetChild(0).GetComponent<ShopElementView>();
        Assert.NotNull(elementView);

        int originalElementAmount = master.Inventory.CheckElementAmount(elementView.ElementModel.PriceKind);

        elementView.gameObject.GetComponentInChildren<Button>().onClick.Invoke();

        yield return new WaitForSecondsRealtime(0.5f);

        master.Inventory.RemoveElement(AlianceCredits, 999);

        Debug.Log(master.Inventory.CheckElementAmount(elementView.ElementModel.ProductKind));
        Debug.Log((originalElementAmount + elementView.ElementModel.ProductAmount));

        Assert.IsTrue(master.Inventory.CheckElementAmount(elementView.ElementModel.ProductKind) == (originalElementAmount + elementView.ElementModel.ProductAmount));
    }
    [UnityTest]
    public IEnumerator TestCanNotBuyItem()
    {
        SceneManager.LoadScene(MasterScene);
        yield return new WaitForSecondsRealtime(3f);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == Initial_Scene);

        MasterSceneManager master = Object.FindObjectOfType<MasterSceneManager>();
        Assert.IsNotNull(master);

        master.Inventory.RemoveElement(AlianceCredits, 999);


        ShopView view = Object.FindObjectOfType<ShopView>();
        view.Initialize();
        Assert.NotNull(view);

        Transform shopItemsParent = GameObject.Find("ShopItems").transform;
        Assert.NotNull(shopItemsParent);

        yield return new WaitForSecondsRealtime(0.5f);

        ShopElementView elementView = shopItemsParent.GetChild(0).GetComponent<ShopElementView>();
        Assert.NotNull(elementView);

        elementView.gameObject.GetComponentInChildren<Button>().onClick.Invoke();
        master.Inventory.AddElement(AlianceCredits, 999);

        yield return new WaitForSecondsRealtime(0.5f);
        GameObject creditsPopUp = GameObject.Find("AlianceCreditsCap_PopUp");
        Assert.IsNotNull(creditsPopUp);


        Assert.IsTrue(creditsPopUp.activeSelf);
    }
}
