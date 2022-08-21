using System.Collections;
using UnityEngine;

public class InitialSceneManager : MonoBehaviour
{
    [SerializeField] private GenericEventBus _DilithiumGenerated;

    private MasterSceneManager _MasterSceneManager;
    private CameraTransitionEffect cameraLogic;

    [SerializeField] private InitialSceneGeneralCanvas canvas;
    [SerializeField] private StarshipAnimationController starship;
    [SerializeField] private BlackCircleTransition blackCircleTransition;

    bool onTransition;

    private void Awake()
    {
        _MasterSceneManager = FindObjectOfType<MasterSceneManager>();

        _DilithiumGenerated.Event += SetDilitiumCanvasAmount;

        cameraLogic = Camera.main.GetComponent<CameraTransitionEffect>();
    }
    private void OnDisable()
    {
        _DilithiumGenerated.Event -= SetDilitiumCanvasAmount;
    }
    private void Start()
    {
        canvas.SetCreditsAmount(_MasterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount);
        SetDilitiumCanvasAmount();
    }
    void SetDilitiumCanvasAmount()
    {
        canvas.SetDilithiumAmount(_MasterSceneManager.runtimeSaveFiles.progres.dilithiumAmount);
    }
    public void EngageOnMission(LevelGridData levelData)
    {
        if (onTransition)
            return;
        onTransition = true;

        if (!_MasterSceneManager.economyManager.CheckDilitiumEmpty())
        {
            _MasterSceneManager.economyManager.UseDilithium();
            _MasterSceneManager.DefineGamePlayLevel(levelData);
            StartCoroutine(CinematicTransition());
        }
        else
        {
            //TODO: Notify No Dilithium Pop Up
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

    void TransitionToGamePlayScene() 
    {
        
        _MasterSceneManager.NavigateToGamePlayScene(); 
    }
}
