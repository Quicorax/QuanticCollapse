using System.Collections;
using UnityEngine;

public class CinematicTransitionManager : MonoBehaviour
{
    private CameraTransitionEffect cameraLogic;

    [SerializeField] private InitialSceneGeneralCanvas canvas;
    [SerializeField] private StarshipView starship;
    [SerializeField] private BlackCircleTransition blackCircleTransition;

    [HideInInspector] public bool onTransition;

    private void Awake()
    {
        cameraLogic = Camera.main.GetComponent<CameraTransitionEffect>();
    }

    public IEnumerator CinematicTransition()
    {
        onTransition = true;
        canvas.CanvasEngageTrigger(true);
        starship.EngageOnMissionAnimation();
        cameraLogic.CameraOnEngageEffect();
        blackCircleTransition.TriggerCircleNarrow();
        yield return new WaitForSeconds(2f);
        onTransition = false;
    }
}
