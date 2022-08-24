using System.Collections;
using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{
    const string Reputation = "Reputation";

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;


    private MasterSceneManager _MasterSceneManager;
    private CameraTransitionEffect cameraLogic;

    [SerializeField] private InitialSceneGeneralCanvas canvas;
    [SerializeField] private StarshipAnimationController starship;
    [SerializeField] private BlackCircleTransition blackCircleTransition;

    bool onTransition;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
        cameraLogic = Camera.main.GetComponent<CameraTransitionEffect>();
    }
    private void OnDisable()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    void SetMasterReference(MasterSceneManager masterReference) { _MasterSceneManager = masterReference; }

    public void EngageOnMission(LevelGridData levelData)
    {
        if (onTransition)
            return;

        if(_MasterSceneManager.Inventory.CheckElementAmount(Reputation) >= levelData.reputationToAcces)
        {
            if (!_MasterSceneManager.Inventory.CheckDilitiumEmpty())
            {
                onTransition = true;
                _MasterSceneManager.Inventory.UseDilithium();
                _MasterSceneManager.DefineGamePlayLevel(levelData);
                StartCoroutine(CinematicTransition());
            }
            else
                canvas.OpenDilithiumPopUp();
        }
        else
            canvas.OpenReputationPopUp();  
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
