public class GameLevelsController
{
    private MasterSceneTransitioner _masterSceneManager;
    private GameProgressionService _gameProgression;
    public GameLevelsController(GameProgressionService gameProgression, MasterSceneTransitioner sceneTransitioner)
    {
        _gameProgression = gameProgression;
        _masterSceneManager = sceneTransitioner;
    }

    public void NavigateToLevel(LevelModel levelModel)
    {
        _gameProgression.UpdateElement("Dilithium", -1);
        _masterSceneManager.DefineGamePlayLevel(levelModel);

        _masterSceneManager.NavigateToGamePlayScene();
    }

}

