using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class MasterSceneManager : MonoBehaviour
{
    const string intialScene = "01_Initial_Scene";
    const string gamePlayScene = "02_GamePlay_Scene";

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    public AudioLogic AudioLogic;
    [HideInInspector] public EconomySystemManager economyManager;
    [HideInInspector] public SerializableSaveData SaveFiles;

    private SaveGameData saveFiles;
    private LevelGridData level;
    [SerializeField] private MasterSceneCanvas canvas;

    private string currentSceneName;

    private void Awake()
    {
        saveFiles = new SaveGameData();
        economyManager = GetComponent<EconomySystemManager>();

        SaveFiles = saveFiles.Load();
    }

    void Start()
    {
        NavigateToInitialScene();
    }

    IEnumerator LoadScene(string _sceneToLoad)
    {
        saveFiles.Save(SaveFiles);
        
        canvas.FadeCanvas(false);

        if (currentSceneName != null)
            SceneManager.UnloadSceneAsync(currentSceneName);

        currentSceneName = _sceneToLoad;

        yield return new WaitForSeconds(0.5f);
        yield return SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);

        canvas.FadeCanvas(true);

        if(level != null)
            SetLevelData();

        _MasterReference.NotifyEvent(this);
    }
    void SetLevelData() 
    {
        _LevelInjected.NotifyEvent(level);
        level = null;
    }

    public void NavigateToInitialScene() { StartCoroutine(LoadScene(intialScene)); }
    public void NavigateToGamePlayScene() { StartCoroutine(LoadScene(gamePlayScene)); }
    public void DefineGamePlayLevel(LevelGridData gamePlayLevel) { level = gamePlayLevel; }

}
