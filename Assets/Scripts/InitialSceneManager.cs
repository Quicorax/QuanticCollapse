using System.Collections;
using UnityEngine;

public class InitialSceneManager : MonoBehaviour
{
    MasterSceneManager masterSceneManager;

    public InitialSceneGeneralCanvas canvas;
    public StarshipAnimationController starship;
    public CameraTransitionEffect camera;
    public BlackCircleTransitionMask blackCircleTransition;
    public void EngageOnMission()
    {
        if (masterSceneManager == null)
            masterSceneManager = FindObjectOfType<MasterSceneManager>();

        StartCoroutine(CinematicTransition());
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

    void TransitionToScene()
    {
        masterSceneManager.NavigateToGamePlayScene();
    }


}
