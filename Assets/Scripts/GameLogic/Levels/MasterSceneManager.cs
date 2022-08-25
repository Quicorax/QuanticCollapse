using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MasterSceneManager : MonoBehaviour
{
    const string intialScene = "01_Initial_Scene";
    const string gamePlayScene = "02_GamePlay_Scene";

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    public AudioLogic AudioLogic;
    public SerializableSaveData SaveFiles;
    [HideInInspector] public InventoryManager Inventory;
    public LevelGridData LevelData;

    private SaveGameData _saveFiles;
    [SerializeField] private MasterSceneCanvas _canvas;

    private string currentSceneName;

    private void Awake()
    {
        _saveFiles = new SaveGameData();
        Inventory = GetComponent<InventoryManager>();

        LoadAll();
    }

    void Start()
    {
        NavigateToInitialScene();
    }

    public void SaveAll() { _saveFiles.Save(SaveFiles); }
    public void LoadAll() { SaveFiles = _saveFiles.Load(); }

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
    void ResetLevelData()
    {
        LevelData = null;
    }

    public void NavigateToInitialScene() { StartCoroutine(LoadScene(intialScene)); }
    public void NavigateToGamePlayScene() { StartCoroutine(LoadScene(gamePlayScene)); }
    public void DefineGamePlayLevel(LevelGridData gamePlayLevel) { LevelData = gamePlayLevel; }

}
