using UnityEngine;

public class GameLevelsController
{
    public GameLevelsModel GameLevelsModel;
    private MasterSceneTransitioner _masterSceneManager;
    private GameProgressionService _gameProgression;
    public GameLevelsController(GameProgressionService gameProgression, MasterSceneTransitioner sceneTransitioner)
    {
        _gameProgression = gameProgression;
        _masterSceneManager = sceneTransitioner;
        LoadLevelsModelData();
    }

    void LoadLevelsModelData()
    {
        GameLevelsModel = JsonUtility.FromJson<GameLevelsModel>(Resources.Load<TextAsset>(Constants.Levels).text);
    }

    public void NavigateToLevel(LevelModel levelModel)
    {
        _gameProgression.UpdateElement(ResourcesType.Dilithium, -1);
        _masterSceneManager.DefineGamePlayLevel(levelModel);

        _masterSceneManager.NavigateToGamePlayScene();
    }

}

