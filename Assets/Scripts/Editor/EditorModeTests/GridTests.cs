using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

public class GridTests
{
    const string Master_Scene_Path = "Assets/Scenes/00_Master_Scene.unity";
    const string GamePlay_Scene_Path = "Assets/Scenes/02_GamePlay_Scene.unity";
    const string Initial_Scene_Path = "Assets/Scenes/01_Initial_Scene.unity";
    public bool TestGenericReference<T>(out T genericObject) where T : Object
    {
        genericObject = Object.FindObjectOfType<T>();
        return genericObject != null;
    }

    //[UnityTest]
    //public IEnumerator TestGridInitializes()
    //{
    //    EditorSceneManager.OpenScene(Master_Scene_Path);
    //    EditorSceneManager.OpenScene(Initial_Scene_Path, OpenSceneMode.Additive);
    //
    //    Assert.IsTrue(TestGenericReference<VirtualGridView>(out VirtualGridView view));
    //
    //    view.Initialize();
    //
    //    Assert.IsTrue(view.Controller.Model.virtualGrid.Count == 9 * 7);
    //}
}
