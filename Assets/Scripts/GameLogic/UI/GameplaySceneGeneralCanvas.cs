using UnityEngine;
using UnityEngine.UI;

public class GameplaySceneGeneralCanvas : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    private MasterSceneManager _MasterSceneManager;

    private AudioLogic audioLogic;

    [SerializeField] private Toggle toggleSFX;
    [SerializeField] private Toggle toggleMusic;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    private void Start()
    {
        toggleSFX.isOn = !_MasterSceneManager.SaveFiles.configuration.isSFXOn;
        toggleMusic.isOn = !_MasterSceneManager.SaveFiles.configuration.isMusicOn;
    }
    void SetMasterReference(MasterSceneManager masterReference)
    {
        _MasterSceneManager = masterReference;
        audioLogic = masterReference.AudioLogic;
    }

    public void RetreatFromMission() { _MasterSceneManager.NavigateToInitialScene(); }
    public void ReplayMission() { _MasterSceneManager.NavigateToGamePlayScene(); }

    public void CancellSFX(bool cancel)
    {
        _MasterSceneManager.SaveFiles.configuration.isSFXOn = !cancel;
        audioLogic.CancellSFXCall(!_MasterSceneManager.SaveFiles.configuration.isSFXOn);
    }

    public void CancellMusic(bool cancel)
    {
        _MasterSceneManager.SaveFiles.configuration.isMusicOn = !cancel;
        audioLogic.CancellMusicCall(!_MasterSceneManager.SaveFiles.configuration.isMusicOn);
    }
}

