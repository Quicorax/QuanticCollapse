using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GridTests
{
    const string Master_Scene_Path = "Assets/Scenes/00_Master_Scene.unity";
    const string GamePlay_Scene_Path = "Assets/Scenes/02_GamePlay_Scene.unity";
    public void TestGetGenericReference<T>(out T genericObject) where T : Object
    {
        genericObject = Object.FindObjectOfType<T>();
        Assert.NotNull(genericObject);
    }


    [Test]
    public void TestVirtualGridInitializes()
    {
        EditorSceneManager.OpenScene(Master_Scene_Path);
        EditorSceneManager.OpenScene(GamePlay_Scene_Path, OpenSceneMode.Additive);

        TestGetGenericReference(out PoolManager pool);
        TestGetGenericReference(out GridView gridView);

        pool.InitializePool();
        gridView.SetLevelData(new());
        gridView.Initialize();

        Assert.AreEqual(pool.spawnedBlocksSoFar, 63);
    }

    [Test]
    public void TestVirtualGridMatch()
    {
        TestVirtualGridInitializes();

        TestGetGenericReference(out PoolManager pool);
        TestGetGenericReference(out GridView gridView);

        gridView.ProcessInput(Vector2Int.one, false);

        Assert.AreEqual(pool.deSpawnedBlocksSoFar, 9);
    }
}
