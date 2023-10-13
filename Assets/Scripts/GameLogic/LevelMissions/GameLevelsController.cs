namespace QuanticCollapse
{
    public class GameLevelsController
    {
        private SceneTransitioner _sceneTransitioner;
        private GameProgressionService _gameProgress;

        public GameLevelsController(GameProgressionService gameProgress, SceneTransitioner sceneTransitioner)
        {
            _gameProgress = gameProgress;
            _sceneTransitioner = sceneTransitioner;
        }

        public void NavigateToLevel(LevelModel levelModel)
        {
            _gameProgress.UpdateElement("Dilithium", -1);
            _sceneTransitioner.DefineGamePlayLevel(levelModel);

            _sceneTransitioner.NavigateToGamePlayScene();
        }
    }
}