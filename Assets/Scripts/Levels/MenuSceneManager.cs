using System.Collections;
using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{
    const string Dilithium = "Dilithium";
    const string Reputation = "Reputation";
    const string AlianceCredits = "AlianceCredits";

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
    void SetDilitiumCanvasAmount() { canvas.SetDilithiumAmount(_MasterSceneManager.Inventory.CheckElementAmount(Dilithium)); }
    void SetReputationCanvasAmount() { canvas.SetReputationAmount(_MasterSceneManager.Inventory.CheckElementAmount(Reputation)); }
    void SetCreditsCanvasAmount() { canvas.SetCreditsAmount(_MasterSceneManager.Inventory.CheckElementAmount(AlianceCredits)); }
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
