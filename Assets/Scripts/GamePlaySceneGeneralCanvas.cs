using UnityEngine;
using UnityEngine.UI;

public class GamePlaySceneGeneralCanvas : MonoBehaviour
{
    public AudioLogic audioLogic;
    MasterSceneManager masterSceneManager;

    public Toggle toggleSFX;
    public Toggle toggleMusic;

    private void Awake()
    {
        masterSceneManager = FindObjectOfType<MasterSceneManager>();
    }
    private void Start()
    {
        toggleSFX.isOn = !masterSceneManager.runtimeSaveFiles.configuration.isSFXOn;
        toggleMusic.isOn = !masterSceneManager.runtimeSaveFiles.configuration.isMusicOn;
    }
    public void RetreatFromMission()
    {
        masterSceneManager.NavigateToInitialScene();
    }
    public void ReplayMission()
    {
        masterSceneManager.NavigateToGamePlayScene();
    }

    public void CancellSFX(bool cancel)
    {
        masterSceneManager.runtimeSaveFiles.configuration.isSFXOn = !cancel;
        audioLogic.CancellSFXCall(!masterSceneManager.runtimeSaveFiles.configuration.isSFXOn);
    }

    public void CancellMusic(bool cancel)
    {
        masterSceneManager.runtimeSaveFiles.configuration.isMusicOn = !cancel;
        audioLogic.CancellMusicCall(!masterSceneManager.runtimeSaveFiles.configuration.isMusicOn);
    }
}

