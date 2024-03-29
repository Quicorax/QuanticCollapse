using System.Collections;
using UnityEngine;

namespace QuanticCollapse
{
    public class CinematicTransitionManager : MonoBehaviour
    {
        private CameraTransitionEffect cameraLogic;

        [SerializeField] 
        private InitialSceneGeneralCanvas canvas;
        [SerializeField] 
        private StarshipVisuals starship;
        [SerializeField] 
        private BlackCircleTransition blackCircleTransition;

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
            blackCircleTransition.TriggerCircleToClose();
            yield return new WaitForSeconds(2f);
            onTransition = false;
        }
    }
}