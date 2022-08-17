using System.Collections;
using UnityEngine;

public class InitialSceneManager : MonoBehaviour
{
    [SerializeField] private GenericEventBus _DilithiumGenerated;

    MasterSceneManager masterSceneManager;

    public InitialSceneGeneralCanvas canvas;
    public StarshipAnimationController starship;
    public CameraTransitionEffect camera;
    public BlackCircleTransitionMask blackCircleTransition;

    private void Awake()
    {
        masterSceneManager = FindObjectOfType<MasterSceneManager>();
        _DilithiumGenerated.Event += SetDilitiumCanvasAmount;
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
    public void EngageOnMission()
    {
        if(!masterSceneManager.economyManager.CheckDilitiumEmpty())
        {
            masterSceneManager.economyManager.UseDilithium();
            StartCoroutine(CinematicTransition());
        }
        else
        {
            //Notify No Dilithium
        }
    }

    IEnumerator CinematicTransition()
    {
        canvas.HideAllElements(true);
        starship.TriggerTransitionAnimation();
        camera.TriggerCameraTransitionEffect();
        blackCircleTransition.TriggerCircleNarrow();
        yield return new WaitForSeconds(2f);
        TransitionToScene();
    }

    void TransitionToScene() { masterSceneManager.NavigateToGamePlayScene(); }
}
