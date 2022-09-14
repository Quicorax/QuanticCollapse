using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MasterSceneTransitioner : MonoBehaviour
{
    const string LobbyScene = "02_Lobby_Scene";
    const string GamePlayScene = "03_GamePlay_Scene";

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    [HideInInspector] public LevelModel LevelData;

    [SerializeField] private LoadingCanvas _canvas;

    private string _currentSceneName;

    void Start()
    {
        NavigateToInitialScene();
    }

    IEnumerator LoadScene(string _sceneToLoad)
    {
        _canvas.FadeCanvas(false);

        if (_currentSceneName != null)
            SceneManager.UnloadSceneAsync(_currentSceneName);

        _currentSceneName = _sceneToLoad;

        yield return new WaitForSeconds(0.5f);
        yield return SceneManager.LoadSceneAsync(_currentSceneName, LoadSceneMode.Additive);

        _canvas.FadeCanvas(true);

        if(LevelData != null)
            SetLevelData();

        _MasterReference.NotifyEvent(this);
    }
    private void SetLevelData() => _LevelInjected.NotifyEvent(LevelData);
    public void NavigateToInitialScene() => StartCoroutine(LoadScene(LobbyScene));
    public void NavigateToGamePlayScene() => StartCoroutine(LoadScene(GamePlayScene));
    public void DefineGamePlayLevel(LevelModel gamePlayLevel) => LevelData = gamePlayLevel;
    public void ResetLevelData() => LevelData = null;

}
