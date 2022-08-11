using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class MasterSceneManager : MonoBehaviour
{
    const string intialScene = "Initial_Scene";
    const string gamePlayScene = "GamePlay_Scene";

    string currentSceneName;
    public CanvasGroup canvasGroup;
    public LoadingIconVisuals rotationIcon;


    void Start()
    {
        NavigateToInitialScene();
    }

    IEnumerator LoadScene(string _sceneToLoad)
    {
        rotationIcon.Init();
        canvasGroup.DOFade(1, 0.5f);

        if (currentSceneName != null)
            SceneManager.UnloadSceneAsync(currentSceneName);

        currentSceneName = _sceneToLoad;

        yield return new WaitForSeconds(.25f);
        yield return SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);

        canvasGroup.DOFade(0, 0.5f).OnComplete(() => rotationIcon.Pause());
    }

    public void NavigateToInitialScene() { StartCoroutine(LoadScene(intialScene)); }
    public void NavigateToGamePlayScene() { StartCoroutine(LoadScene(gamePlayScene)); }
}
