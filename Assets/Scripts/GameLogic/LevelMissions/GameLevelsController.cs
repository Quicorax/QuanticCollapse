using UnityEngine;

public class GameLevelsController
{
    const string Dilithium = "Dilithium";

    public GameLevelsModel GameLevelsModel;
    private MasterSceneManager _MasterSceneManager;
    public GameLevelsController(MasterSceneManager master)
    {
        _MasterSceneManager = master;
        LoadLevelsModelData();
    }

    void LoadLevelsModelData()
    {
        GameLevelsModel = JsonUtility.FromJson<GameLevelsModel>(Resources.Load<TextAsset>("Levels").text);
    }

    public void NavigateToLevel(LevelModel levelModel)
    {
        _MasterSceneManager.Inventory.RemoveElement(Dilithium, 1);
        _MasterSceneManager.DefineGamePlayLevel(levelModel);

        _MasterSceneManager.NavigateToGamePlayScene();
    }

}

