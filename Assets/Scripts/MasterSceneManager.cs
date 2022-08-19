using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class MasterSceneManager : MonoBehaviour
{
    const string intialScene = "Initial_Scene";
    const string gamePlayScene = "GamePlay_Scene";

    private SaveGameData saveFiles;
    [HideInInspector] public EconomySystemManager economyManager;

    private string currentSceneName;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private LoadingIconVisuals rotationIcon;

    [HideInInspector] public SerializableSaveData runtimeSaveFiles;

    LevelGridData level;

    private void Awake()
    {
        saveFiles = new SaveGameData();
        economyManager = GetComponent<EconomySystemManager>();

        runtimeSaveFiles = saveFiles.Load();
    }
    void Start()
    {
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

        if(level != null)
            InjectLevelData();

    }
    void InjectLevelData() 
    { 
        FindObjectOfType<LevelDataHolder>().LevelData = level;
        level = null;
    }

    public void NavigateToInitialScene() { StartCoroutine(LoadScene(intialScene)); }
    public void NavigateToGamePlayScene() { StartCoroutine(LoadScene(gamePlayScene)); }
    public void DefineGamePlayLevel(LevelGridData gamePlayLevel) { level = gamePlayLevel; }

}
