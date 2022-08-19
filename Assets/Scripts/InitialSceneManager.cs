using System.Collections;
using UnityEngine;

public class InitialSceneManager : MonoBehaviour
{
    [SerializeField] private GenericEventBus _DilithiumGenerated;

    private MasterSceneManager masterSceneManager;
    private CameraTransitionEffect cameraLogic;

    [SerializeField] private InitialSceneGeneralCanvas canvas;
    [SerializeField] private StarshipAnimationController starship;
    [SerializeField] private BlackCircleTransition blackCircleTransition;

    bool onTransition;

    private void Awake()
    {
        _DilithiumGenerated.Event += SetDilitiumCanvasAmount;

        masterSceneManager = FindObjectOfType<MasterSceneManager>();
        cameraLogic = Camera.main.GetComponent<CameraTransitionEffect>();
    }
    private void OnDisable()
    {
        _DilithiumGenerated.Event -= SetDilitiumCanvasAmount;
    }
    private void Start()
    {
        canvas.SetCreditsAmount(masterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount);
        SetDilitiumCanvasAmount();
    }

    void SetDilitiumCanvasAmount()
    {
        canvas.SetDilithiumAmount(masterSceneManager.runtimeSaveFiles.progres.dilithiumAmount);
    }
    public void EngageOnMission(LevelGridData levelData)
    {
        if (onTransition)
            return;
        onTransition = true;

        if (!masterSceneManager.economyManager.CheckDilitiumEmpty())
        {
            masterSceneManager.economyManager.UseDilithium();
            masterSceneManager.DefineGamePlayLevel(levelData);
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
        
        masterSceneManager.NavigateToGamePlayScene(); 
    }
}
