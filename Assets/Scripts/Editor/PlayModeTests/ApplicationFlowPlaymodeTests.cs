using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class ApplicationFlowPlaymodeTests
{
    //const string MasterScene = "00_Master_Scene";
    //const string Initial_Scene = "01_Initial_Scene";
    //const string GamePlay_Scene = "02_GamePlay_Scene";
    //
    //const string Dilithium = "Dilithium";
    //const string Reputation = "Reputation";
    //
    //const string LevelView = "LevelView_";
    //
    //public void TestGetGenericReference<T>(out T genericObject) where T : Object
    //{
    //    genericObject = Object.FindObjectOfType<T>();
    //    Assert.IsNotNull(genericObject);
    //}
    //
    //[UnityTest]
    //public IEnumerator TestInitialSceneLoad()
    //{
    //    SceneManager.LoadScene(MasterScene);
    //    yield return new WaitForSecondsRealtime(1f);
    //
    //    Assert.AreEqual(SceneManager.GetSceneAt(1).name, Initial_Scene);
    //}
    //
    //[UnityTest]
    //public IEnumerator TestCanEngageOnMission()
    //{
    //    yield return TestInitialSceneLoad();
    //
    //    TestGetGenericReference(out MasterSceneManager master);
    //
    //    master.Inventory.AddElement(Dilithium, 9999);
    //    master.Inventory.AddElement(Reputation, 9999);
    //
    //    TestGetGenericReference(out CinematicTransitionManager menuManager);
    //
    //    Button mission0Button = GameObject.Find(LevelView + 0).GetComponent<Button>();
    //
    //    Assert.IsNotNull(mission0Button);
    //    mission0Button.onClick.Invoke();
    //
    //    master.Inventory.RemoveElement(Dilithium, 9999);
    //    master.Inventory.RemoveElement(Reputation, 9999);
    //
    //    yield return new WaitForSecondsRealtime(0.75f);
    //
    //    Assert.IsTrue(menuManager.onTransition);
    //}
    //
    //[UnityTest]
    //public IEnumerator TestCanNotEngageOnMissionByReputation()
    //{
    //    yield return TestInitialSceneLoad();
    //
    //    TestGetGenericReference(out MasterSceneManager master);
    //
    //    master.Inventory.RemoveElement(Reputation, 9999);
    //    master.Inventory.AddElement(Dilithium, 9999);
    //
    //    Button mission0Button = GameObject.Find(LevelView + 0).GetComponent<Button>();
    //    Assert.IsNotNull(mission0Button);
    //    mission0Button.onClick.Invoke();
    //
    //    master.Inventory.AddElement(Reputation, 9999);
    //    master.Inventory.RemoveElement(Dilithium, 9999);
    //
    //    yield return new WaitForSecondsRealtime(0.2f);
    //    GameObject reputationPopUpImage = GameObject.Find("IconImage");
    //    Assert.IsNotNull(reputationPopUpImage);
    //
    //    Assert.AreEqual(reputationPopUpImage.GetComponent<Image>().sprite.name, Reputation);
    //}
    //
    //[UnityTest]
    //public IEnumerator TestCanNotEngageOnMissionByDilithium()
    //{
    //    yield return TestInitialSceneLoad();
    //
    //    TestGetGenericReference(out MasterSceneManager master);
    //
    //    master.Inventory.AddElement(Reputation, 9999);
    //    master.Inventory.RemoveElement(Dilithium, 9999);
    //
    //    Button mission0Button = GameObject.Find(LevelView + 0).GetComponent<Button>();
    //    Assert.IsNotNull(mission0Button);
    //    mission0Button.onClick.Invoke();
    //
    //    master.Inventory.RemoveElement(Reputation, 9999);
    //    master.Inventory.AddElement(Dilithium, 9999);
    //
    //    yield return new WaitForSecondsRealtime(0.2f);
    //    GameObject dilithiumPopUpIcon = GameObject.Find("IconImage");
    //    Assert.IsNotNull(dilithiumPopUpIcon);
    //
    //    Assert.AreEqual(dilithiumPopUpIcon.GetComponent<Image>().sprite.name, Dilithium);
    //}
    //
    //[UnityTest]
    //public IEnumerator TestCanLoadGamePlaySceneWithLevelData()
    //{
    //    yield return TestInitialSceneLoad();
    //
    //    TestGetGenericReference(out MasterSceneManager master);
    //
    //    master.Inventory.AddElement(Reputation, 9999);
    //    master.Inventory.AddElement(Dilithium, 9999);
    //
    //    Button mission0Button = GameObject.Find(LevelView + 3).GetComponent<Button>();
    //    Assert.IsNotNull(mission0Button);
    //
    //    mission0Button.onClick.Invoke();
    //
    //    master.Inventory.RemoveElement(Reputation, 9999);
    //    master.Inventory.RemoveElement(Dilithium, 9999);
    //
    //    yield return new WaitForSecondsRealtime(3f);
    //
    //    Assert.AreEqual(SceneManager.GetSceneAt(1).name, GamePlay_Scene);
    //
    //    TestGetGenericReference(out GameplayRewards gameplaySceneManager);
    //
    //    Assert.AreEqual(master.LevelData, gameplaySceneManager.LevelData);
    //}
}
