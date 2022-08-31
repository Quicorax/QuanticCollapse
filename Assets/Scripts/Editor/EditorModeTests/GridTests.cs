using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Linq;
using UnityEngine.TestTools;

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
    public void TestInitializeExternalBoosters()
    {
        TestVirtualGridInitializes();

        TestGetGenericReference(out InventoryManager Inventory);
        TestGetGenericReference(out MasterSceneManager Master);
        TestGetGenericReference(out ExternalBoosterView ExternalBoostersView);

        Inventory.SaveFiles = new SaveGameData().Load();
        Master.Inventory = Inventory;

        ExternalBoostersView.SetMasterReference(Master);
        ExternalBoostersView.Initialize();

        Assert.NotZero(ExternalBoostersView.ActiveExternalBoosters.Count);
        Assert.AreEqual(ExternalBoostersView.ActiveExternalBoosters.Count, ExternalBoostersView.ExternalBooster.Count);
    }

    [Test]
    public void TestCanUseExternalBoosterFirstAidKit()
    {
        TestInitializeExternalBoosters();

        TestGetGenericReference(out ExternalBoosterView ExternalBoostersView);
        TestGetGenericReference(out PoolManager pool);          
        TestGetGenericReference(out GridView gridView);

        ExternalBoosterElementView externalBoosterView = ExternalBoostersView.ActiveExternalBoosters.Find(booster => booster.name == "FirstAidKit");
        Assert.NotNull(externalBoosterView);

        gridView.Controller.Model.PlayerLife -= 10;

        int preBoosterPlayerLife = gridView.Controller.Model.PlayerLife;

        externalBoosterView.ExecuteBooster();

        int postBoosterPlayerLife = gridView.Controller.Model.PlayerLife;

        Assert.Less(preBoosterPlayerLife, postBoosterPlayerLife);
    }
    [Test]
    public void TestCanUseExternalBoosterEasyTrigger()
    {
        TestInitializeExternalBoosters();

        TestGetGenericReference(out ExternalBoosterView ExternalBoostersView);
        TestGetGenericReference(out PoolManager pool);
        TestGetGenericReference(out GridView gridView);

        ExternalBoosterElementView externalBoosterView = ExternalBoostersView.ActiveExternalBoosters.Find(booster => booster.name == "EasyTrigger");
        Assert.NotNull(externalBoosterView);

        int preBoosterEnemyLife = gridView.Controller.Model.EnemyLife;

        externalBoosterView.ExecuteBooster();

        int postBoosterEnemyLife = gridView.Controller.Model.EnemyLife;

        Assert.Less(postBoosterEnemyLife, preBoosterEnemyLife);
    }
    [Test]
    public void TestCanUseExternalBoosterDeAthomizer()
    {
        TestInitializeExternalBoosters();

        TestGetGenericReference(out ExternalBoosterView ExternalBoostersView);
        TestGetGenericReference(out PoolManager pool);
        TestGetGenericReference(out GridView gridView);
        TestGetGenericReference(out UserInputManager input);

        ExternalBoosterElementView externalBoosterView = ExternalBoostersView.ActiveExternalBoosters.Find(booster => booster.name == "DeAthomizer");
        Assert.NotNull(externalBoosterView);

        externalBoosterView.ExecuteBooster();

        Assert.IsTrue(input.deAthomizerBoostedInput);

        gridView.ProcessInput(Vector2Int.one, true);

        Assert.AreEqual(pool.deSpawnedBlocksSoFar, 1);
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

    //[Test]
    //public void TestBoosterRowColumnGenerated()
    //{
    //    TestVirtualGridInitializes();
    //
    //    TestGetGenericReference(out VirtualGridView gridView);
    //
    //    gridView.ProcessInput(Vector2Int.one, false);
    //
    //    Assert.AreEqual(gridView.Controller.Model.virtualGrid[Vector2Int.zero].BlockModel.Booster.boosterKind, ElementKind.BoosterRowColumn);
    //}
}
