
namespace QuanticCollapse
{
    public class GameLevelsController
    {
        private SceneTransitioner _sceneTransitioner;
        private GameProgressionService _gameProgression;
        public GameLevelsController(GameProgressionService gameProgression, SceneTransitioner sceneTransitioner)
        {
            _gameProgression = gameProgression;
            _sceneTransitioner = sceneTransitioner;
        }

        public void NavigateToLevel(LevelModel levelModel)
        {
            _gameProgression.UpdateElement("Dilithium", -1);
            _sceneTransitioner.DefineGamePlayLevel(levelModel);

            _sceneTransitioner.NavigateToGamePlayScene();
        }

    }

}