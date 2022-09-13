using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MasterSceneManager : MonoBehaviour
{
    const string LobbyScene = "02_Lobby_Scene";
    const string GamePlayScene = "03_GamePlay_Scene";

    [SerializeField]  private GenericEventBus _ElementAmountModified;

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    public AudioLogic AudioLogic;

    public SerializableSaveData SaveFiles;
    [HideInInspector] public Inventory Inventory;

    [HideInInspector] public LevelModel LevelData;

    private SaveGameData _saveFiles;
    [SerializeField] private LoadingCanvas _canvas;

    private string _currentSceneName;
    private void Awake()
    {
        _saveFiles = new();
        LoadAll();

        Inventory = new(SaveFiles, _ElementAmountModified);
    }

    void Start()
    {
        NavigateToInitialScene();
    }
    public void SaveAll() => _saveFiles.Save(SaveFiles);
    public void LoadAll() => SaveFiles = _saveFiles.Load();

    IEnumerator LoadScene(string _sceneToLoad)
    {
        SaveAll();
        
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
    void SetLevelData() 
    {
        _LevelInjected.NotifyEvent(LevelData);
    }


    public void NavigateToInitialScene() => StartCoroutine(LoadScene(LobbyScene));
    public void NavigateToGamePlayScene() => StartCoroutine(LoadScene(GamePlayScene));
    public void DefineGamePlayLevel(LevelModel gamePlayLevel) => LevelData = gamePlayLevel;
    public void ResetLevelData() => LevelData = null;

}
