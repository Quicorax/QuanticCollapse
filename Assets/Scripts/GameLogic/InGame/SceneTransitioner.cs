using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace QuanticCollapse
{
    public class SceneTransitioner : MonoBehaviour
    {
        [SerializeField] private SendSceneTransitionerReferenceEventBus _SceneTransitionerReference;
        [SerializeField] private LevelInjectedEventBus _LevelInjected;

        [HideInInspector] public LevelModel LevelData;

        [SerializeField] private LoadingCanvas _canvas;

        private string _currentSceneName;

        public void NavigateToInitialScene() => StartCoroutine(LoadScene("02_Lobby_Scene"));
        public void NavigateToGamePlayScene() => StartCoroutine(LoadScene("03_GamePlay_Scene"));
        public void DefineGamePlayLevel(LevelModel gamePlayLevel) => LevelData = gamePlayLevel;
        public void ResetLevelData() => LevelData = null;

        private void Start()
        {
            NavigateToInitialScene();
        }

        private IEnumerator LoadScene(string _sceneToLoad)
        {
            _canvas.FadeCanvas(false);

            if (_currentSceneName != null)
                SceneManager.UnloadSceneAsync(_currentSceneName);

            _currentSceneName = _sceneToLoad;

            yield return new WaitForSeconds(0.5f);
            yield return SceneManager.LoadSceneAsync(_currentSceneName, LoadSceneMode.Additive);

            _canvas.FadeCanvas(true);

            if (LevelData != null)
            {
                SetLevelData();
            }

            _SceneTransitionerReference.NotifyEvent(this);
        }

        private void SetLevelData() => _LevelInjected.NotifyEvent(LevelData);
    }
}