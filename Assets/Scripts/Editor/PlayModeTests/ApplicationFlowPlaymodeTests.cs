using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class ApplicationFlowPlaymodeTests
{
    const string MasterScene = "00_Master_Scene";
    const string Initial_Scene = "01_Initial_Scene";
    const string GamePlay_Scene = "02_GamePlay_Scene";

    const string Dilithium = "Dilithium";
    const string Reputation = "Reputation";

    public bool TestGenericReference<T>(out T genericObject) where T : Object
    {
        genericObject = Object.FindObjectOfType<T>();
        return genericObject != null;
    }

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
        yield return TestInitialSceneLoad();

        Assert.IsTrue(TestGenericReference<MasterSceneManager>(out MasterSceneManager master));

        master.Inventory.AddElement(Dilithium, 9999);
        master.Inventory.AddElement(Reputation, 9999);

        Assert.IsTrue(TestGenericReference<CinematicTransitionManager>(out CinematicTransitionManager menuManager));

        Button mission0Button = GameObject.Find("Mission (0)").GetComponent<Button>();
        Assert.IsNotNull(mission0Button);
        mission0Button.onClick.Invoke();

        master.Inventory.RemoveElement(Dilithium, 9999);
        master.Inventory.RemoveElement(Reputation, 9999);

        yield return new WaitForSecondsRealtime(0.75f);

        Assert.IsTrue(menuManager.onTransition);
    }

    [UnityTest]
    public IEnumerator TestCanNotEngageOnMissionByReputation()
    {
        yield return TestInitialSceneLoad();

        Assert.IsTrue(TestGenericReference<MasterSceneManager>(out MasterSceneManager master));

        master.Inventory.RemoveElement(Reputation, 9999);
        master.Inventory.AddElement(Dilithium, 9999);

        Button mission0Button = GameObject.Find("Mission (0)").GetComponent<Button>();
        Assert.IsNotNull(mission0Button);
        mission0Button.onClick.Invoke();

        master.Inventory.AddElement(Reputation, 9999);
        master.Inventory.RemoveElement(Dilithium, 9999);

        yield return new WaitForSecondsRealtime(0.2f);
        GameObject reputationPopUp = GameObject.Find("ReputationCap_PopUp");
        Assert.IsNotNull(reputationPopUp);

        Assert.IsTrue(reputationPopUp.activeSelf);
    }

    [UnityTest]
    public IEnumerator TestCanNotEngageOnMissionByDilithium()
    {
        yield return TestInitialSceneLoad();

        Assert.IsTrue(TestGenericReference<MasterSceneManager>(out MasterSceneManager master));

        master.Inventory.AddElement(Reputation, 9999);
        master.Inventory.RemoveElement(Dilithium, 9999);

        Button mission0Button = GameObject.Find("Mission (0)").GetComponent<Button>();
        Assert.IsNotNull(mission0Button);
        mission0Button.onClick.Invoke();

        master.Inventory.RemoveElement(Reputation, 9999);
        master.Inventory.AddElement(Dilithium, 9999);

        yield return new WaitForSecondsRealtime(0.2f);
        GameObject dilithiumPopUp = GameObject.Find("DilithiumCap_PopUp");
        Assert.IsNotNull(dilithiumPopUp);

        Assert.IsTrue(dilithiumPopUp.activeSelf);
    }

    [UnityTest]
    public IEnumerator TestCanLoadGamePlaySceneWithLevelData()
    {
        yield return TestInitialSceneLoad();

        Assert.IsTrue(TestGenericReference<MasterSceneManager>(out MasterSceneManager master));

        master.Inventory.AddElement(Reputation, 9999);
        master.Inventory.AddElement(Dilithium, 9999);

        Button mission0Button = GameObject.Find("Mission (3)").GetComponent<Button>();
        Assert.IsNotNull(mission0Button);

        mission0Button.onClick.Invoke();

        master.Inventory.RemoveElement(Reputation, 9999);
        master.Inventory.RemoveElement(Dilithium, 9999);

        yield return new WaitForSecondsRealtime(3f);

        Assert.IsTrue(SceneManager.GetSceneAt(1).name == GamePlay_Scene);

        Assert.IsTrue(TestGenericReference<GameplaySceneManager>(out GameplaySceneManager gameplaySceneManager));

        Assert.IsTrue(master.LevelData == gameplaySceneManager.LevelData);
    }
}
