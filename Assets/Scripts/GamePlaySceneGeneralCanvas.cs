using UnityEngine;
using UnityEngine.UI;

public class GamePlaySceneGeneralCanvas : MonoBehaviour
{
    private MasterSceneManager _MasterSceneManager;

    [SerializeField] private AudioLogic audioLogic;

    [SerializeField] private Toggle toggleSFX;
    [SerializeField] private Toggle toggleMusic;

    private void Awake()
    {
        _MasterSceneManager = FindObjectOfType<MasterSceneManager>();
    }
    private void Start()
    {
        toggleSFX.isOn = !_MasterSceneManager.runtimeSaveFiles.configuration.isSFXOn;
        toggleMusic.isOn = !_MasterSceneManager.runtimeSaveFiles.configuration.isMusicOn;
    }
    public void RetreatFromMission()
    {
        _MasterSceneManager.NavigateToInitialScene();
    }
    public void ReplayMission()
    {
        _MasterSceneManager.NavigateToGamePlayScene();
    }

    public void CancellSFX(bool cancel)
    {
        _MasterSceneManager.runtimeSaveFiles.configuration.isSFXOn = !cancel;
        audioLogic.CancellSFXCall(!_MasterSceneManager.runtimeSaveFiles.configuration.isSFXOn);
    }

    public void CancellMusic(bool cancel)
    {
        _MasterSceneManager.runtimeSaveFiles.configuration.isMusicOn = !cancel;
        audioLogic.CancellMusicCall(!_MasterSceneManager.runtimeSaveFiles.configuration.isMusicOn);
    }
}

