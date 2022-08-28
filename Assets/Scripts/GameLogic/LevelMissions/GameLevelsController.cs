using UnityEngine;

public class GameLevelsController
{
    public GameLevelsModel GameLevelsModel;

    public GameLevelsController()
    {
        LoadLevelsModelData();
    }

    void LoadLevelsModelData()
    {
        GameLevelsModel = JsonUtility.FromJson<GameLevelsModel>(Resources.Load<TextAsset>("Levels").text);
    }

    public void NavigateToLevel(LevelModel levelModel)
    {
        //Scene loading logic
    }

}

