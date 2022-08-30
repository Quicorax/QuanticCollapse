using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;

public class GameLevelsTests
{
    const string Initial_Scene_Path = "Assets/Scenes/01_Initial_Scene.unity";
    const string levelsURL = "https://script.google.com/macros/s/AKfycbwTDEcnhXzTA9jgERQsMcI7QdR7JT9PGmuQMhiPao3jLgmcVFIbJgZMh-PRBglhsGAM/exec";    
    public void TestGetGenericReference<T>(out T genericObject) where T : Object
    {
        genericObject = Object.FindObjectOfType<T>();
        Assert.IsNotNull(genericObject);
    }

    [Test]
    public void LevelSelectorInitializes()
    {
        EditorSceneManager.OpenScene(Initial_Scene_Path);

        TestGetGenericReference(out GameLevelsView view);
        view.Initialize();

        Transform gameLevelsParent = GameObject.Find("MissionsLayout").transform;
        Assert.NotNull(gameLevelsParent);

        Assert.NotZero(gameLevelsParent.childCount); 
        Assert.AreEqual(gameLevelsParent.childCount, view.GameLevelsController.GameLevelsModel.Levels.Count);
    }

    [UnityTest]
    public IEnumerator LevelSelectorIsUpdated()
    {
        string localLevelsModel = Resources.Load<TextAsset>("Levels").text;
        string cloudLevelsModel = string.Empty;

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

            cloudLevelsModel = request.downloadHandler.text;
        };

        yield return new WaitUntil(()=> webRecuestCompleted);

        Assert.AreEqual(localLevelsModel, cloudLevelsModel);
    }
}
