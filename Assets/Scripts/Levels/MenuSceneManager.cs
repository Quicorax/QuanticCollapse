using System.Collections;
using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private GenericEventBus _DilithiumGenerated;

    private MasterSceneManager _MasterSceneManager;
    private CameraTransitionEffect cameraLogic;

    [SerializeField] private InitialSceneGeneralCanvas canvas;
    [SerializeField] private StarshipAnimationController starship;
    [SerializeField] private BlackCircleTransition blackCircleTransition;

    bool onTransition;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
        _DilithiumGenerated.Event += SetDilitiumCanvasAmount;

        cameraLogic = Camera.main.GetComponent<CameraTransitionEffect>();
    }
    private void OnDisable()
    {
        _DilithiumGenerated.Event -= SetDilitiumCanvasAmount;
        _MasterReference.Event -= SetMasterReference;
    }
    private void Start()
    {
        SetCreditsCanvasAmount();
        SetDilitiumCanvasAmount();
        SetReputationCanvasAmount();
    }
    void SetMasterReference(MasterSceneManager masterReference) { _MasterSceneManager = masterReference; }
    void SetDilitiumCanvasAmount() { canvas.SetDilithiumAmount(_MasterSceneManager.SaveFiles.progres.dilithiumAmount); }
    void SetReputationCanvasAmount() { canvas.SetReputationAmount(_MasterSceneManager.SaveFiles.progres.reputation); }
    void SetCreditsCanvasAmount() { canvas.SetCreditsAmount(_MasterSceneManager.SaveFiles.progres.alianceCreditsAmount); }
    public void EngageOnMission(LevelGridData levelData)
    {
        if (onTransition)
            return;

        if(_MasterSceneManager.SaveFiles.progres.reputation >= levelData.reputationToAcces)
        {

            if (!_MasterSceneManager.economyManager.CheckDilitiumEmpty())
            {
                onTransition = true;
                _MasterSceneManager.economyManager.UseDilithium();
                _MasterSceneManager.DefineGamePlayLevel(levelData);
                StartCoroutine(CinematicTransition());
            }
            else
            {
                canvas.OpenDilithiumPopUp();
            }
        }
        else
        {
            canvas.OpenReputationPopUp();  
        }
    }

    IEnumerator CinematicTransition()
    {
        canvas.CanvasEngageTrigger(true);
        starship.TriggerTransitionAnimation();
        cameraLogic.TriggerCameraTransitionEffect();
        blackCircleTransition.TriggerCircleNarrow();
        yield return new WaitForSeconds(2f);
        TransitionToGamePlayScene();
        onTransition = false;
    }

    void TransitionToGamePlayScene() { _MasterSceneManager.NavigateToGamePlayScene(); }
}
