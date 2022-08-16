using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class MasterSceneManager : MonoBehaviour
{
    const string intialScene = "Initial_Scene";
    const string gamePlayScene = "GamePlay_Scene";

    private SaveGameData saveFiles;

    private string currentSceneName;
    public CanvasGroup canvasGroup;
    public LoadingIconVisuals rotationIcon;

    public SerializableSaveData runtimeSaveFiles;

    private void Awake()
    {
        saveFiles = new SaveGameData();
    }
    void Start()
    {
        runtimeSaveFiles = saveFiles.Load();

        NavigateToInitialScene();
    }

    IEnumerator LoadScene(string _sceneToLoad)
    {
        saveFiles.Save(runtimeSaveFiles);
        
        rotationIcon.Init();
        canvasGroup.DOFade(1, 0.5f);

        if (currentSceneName != null)
            SceneManager.UnloadSceneAsync(currentSceneName);

        currentSceneName = _sceneToLoad;

        yield return new WaitForSeconds(0.25f);
        yield return SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);

        canvasGroup.DOFade(0, 0.5f).OnComplete(() => rotationIcon.Pause());
    }

    public void NavigateToInitialScene() { StartCoroutine(LoadScene(intialScene)); }
    public void NavigateToGamePlayScene() { StartCoroutine(LoadScene(gamePlayScene)); }
}
