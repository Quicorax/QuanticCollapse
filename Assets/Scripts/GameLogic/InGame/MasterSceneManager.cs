using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MasterSceneManager : MonoBehaviour
{
    const string intialScene = "01_Initial_Scene";
    const string gamePlayScene = "02_GamePlay_Scene";

    [SerializeField]  private GenericEventBus _ElementAmountModified;

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    public AudioLogic AudioLogic;
    public SerializableSaveData SaveFiles;
    [HideInInspector] public Inventory Inventory;
    [HideInInspector] public LevelModel LevelData;

    private SaveGameData _saveFiles;
    [SerializeField] private MasterSceneCanvas _canvas;

    private string currentSceneName;
    private void Awake()
    {
        _saveFiles = new();
        LoadAll();

        Inventory = new(SaveFiles, _ElementAmountModified);
    }

    void Start()
    {
        NavigateToInitialScene();
        //NavigateToTestScene();
    }
    public void SaveAll() => _saveFiles.Save(SaveFiles);
    public void LoadAll() => SaveFiles = _saveFiles.Load();

    IEnumerator LoadScene(string _sceneToLoad)
    {
        SaveAll();
        
        _canvas.FadeCanvas(false);

        if (currentSceneName != null)
            SceneManager.UnloadSceneAsync(currentSceneName);

        currentSceneName = _sceneToLoad;

        yield return new WaitForSeconds(0.5f);
        yield return SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);

        _canvas.FadeCanvas(true);

        if(LevelData != null)
            SetLevelData();

        _MasterReference.NotifyEvent(this);
    }
    void SetLevelData() 
    {
        _LevelInjected.NotifyEvent(LevelData);
        Invoke(nameof(ResetLevelData), 0.5f);
    }


    public void NavigateToInitialScene() => StartCoroutine(LoadScene(intialScene));
    //public void NavigateToTestScene() => StartCoroutine(LoadScene("Test_Scene"));
    public void NavigateToGamePlayScene() => StartCoroutine(LoadScene(gamePlayScene));
    public void DefineGamePlayLevel(LevelModel gamePlayLevel) => LevelData = gamePlayLevel;
    void ResetLevelData() => LevelData = null;

}
