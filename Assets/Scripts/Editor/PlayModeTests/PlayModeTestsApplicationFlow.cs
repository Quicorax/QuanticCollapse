using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class PlayModeTestsApplicationFlow
{
    const string MasterScene = "00_MasterScene";
    const string Initial_Scene = "01_Initial_Scene";
    const string GamePlay_Scene = "02_GamePlay_Scene";
    const string Dilithium = "Dilithium";
    const string Reputation = "Reputation";

    [UnityTest]
    public IEnumerator TestInitialSceneLoad()
    {
        SceneManager.LoadScene(MasterScene);
        yield return new WaitForSecondsRealtime(1f);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == Initial_Scene);
    }

    [UnityTest]
    public IEnumerator TestCanEngageOnMission()
    {
        SceneManager.LoadScene(MasterScene);
        yield return new WaitForSecondsRealtime(3f);

        MasterSceneManager master = Object.FindObjectOfType<MasterSceneManager>();
        Assert.IsNotNull(master);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == Initial_Scene);

        master.Inventory.AddElement(Dilithium, 999);
        master.Inventory.AddElement(Reputation, 999);

        MenuSceneManager menuManager = Object.FindObjectOfType<MenuSceneManager>();
        Assert.IsNotNull(menuManager);

        Button mission0Button = GameObject.Find("Mission (0)").GetComponent<Button>();
        Assert.IsNotNull(mission0Button);
        mission0Button.onClick.Invoke();

        master.Inventory.RemoveElement(Dilithium, 999);
        master.Inventory.RemoveElement(Reputation, 999);

        yield return new WaitForSecondsRealtime(0.75f);

        Assert.IsTrue(menuManager.onTransition);
    }

    [UnityTest]
    public IEnumerator TestCanNotEngageOnMissionByReputation()
    {
        SceneManager.LoadScene(MasterScene);
        yield return new WaitForSecondsRealtime(1f);

        MasterSceneManager master = Object.FindObjectOfType<MasterSceneManager>();
        Assert.IsNotNull(master);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == Initial_Scene);

        master.Inventory.RemoveElement(Reputation, 999);
        master.Inventory.AddElement(Dilithium, 999);

        yield return new WaitForSecondsRealtime(0.5f);
        Button mission0Button = GameObject.Find("Mission (0)").GetComponent<Button>();
        Assert.IsNotNull(mission0Button);
        mission0Button.onClick.Invoke();

        master.Inventory.AddElement(Reputation, 999);
        master.Inventory.RemoveElement(Dilithium, 999);

        yield return new WaitForSecondsRealtime(0.2f);
        GameObject reputationPopUp = GameObject.Find("ReputationCap_PopUp");
        Assert.IsNotNull(reputationPopUp);

        Assert.IsTrue(reputationPopUp.activeSelf);
    }

    [UnityTest]
    public IEnumerator TestCanNotEngageOnMissionByDilithium()
    {
        SceneManager.LoadScene(MasterScene);
        yield return new WaitForSecondsRealtime(1f);


        MasterSceneManager master = Object.FindObjectOfType<MasterSceneManager>();
        Assert.IsNotNull(master);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == Initial_Scene);

        master.Inventory.AddElement(Reputation, 999);
        master.Inventory.RemoveElement(Dilithium, 999);

        yield return new WaitForSecondsRealtime(0.5f);
        Button mission0Button = GameObject.Find("Mission (0)").GetComponent<Button>();
        Assert.IsNotNull(mission0Button);
        mission0Button.onClick.Invoke();

        master.Inventory.RemoveElement(Reputation, 999);
        master.Inventory.AddElement(Dilithium, 999);

        yield return new WaitForSecondsRealtime(0.2f);
        GameObject dilithiumPopUp = GameObject.Find("DilithiumCap_PopUp");
        Assert.IsNotNull(dilithiumPopUp);

        Assert.IsTrue(dilithiumPopUp.activeSelf);
    }

    [UnityTest]
    public IEnumerator TestCanLoadGamePlaySceneWithLevelData()
    {
        SceneManager.LoadScene(MasterScene);
        yield return new WaitForSecondsRealtime(1f);


        MasterSceneManager master = Object.FindObjectOfType<MasterSceneManager>();
        Assert.IsNotNull(master);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == Initial_Scene);

        master.Inventory.AddElement(Reputation, 999);
        master.Inventory.AddElement(Dilithium, 999);

        yield return new WaitForSecondsRealtime(0.5f);
        Button mission0Button = GameObject.Find("Mission (3)").GetComponent<Button>();
        Assert.IsNotNull(mission0Button);

        mission0Button.onClick.Invoke();

        master.Inventory.RemoveElement(Reputation, 999);
        master.Inventory.RemoveElement(Dilithium, 999);

        yield return new WaitForSecondsRealtime(3f);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == GamePlay_Scene);

        GameplaySceneManager gameplaySceneManager = Object.FindObjectOfType<GameplaySceneManager>();
        Assert.IsNotNull(gameplaySceneManager);

        Assert.IsTrue(master.LevelData == gameplaySceneManager.LevelData);
    }
}
