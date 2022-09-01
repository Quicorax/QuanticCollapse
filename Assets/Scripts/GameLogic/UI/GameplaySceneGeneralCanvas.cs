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
        toggleSFX.isOn = !_MasterSceneManager.SaveFiles.Configuration.IsSFXOn;
        toggleMusic.isOn = !_MasterSceneManager.SaveFiles.Configuration.IsMusicOn;
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
        _MasterSceneManager.SaveFiles.Configuration.IsSFXOn = !cancel;
        audioLogic.CancellSFXCall(!_MasterSceneManager.SaveFiles.Configuration.IsSFXOn);
    }

    public void CancellMusic(bool cancel)
    {
        _MasterSceneManager.SaveFiles.Configuration.IsMusicOn = !cancel;
        audioLogic.CancellMusicCall(!_MasterSceneManager.SaveFiles.Configuration.IsMusicOn);
    }
}

