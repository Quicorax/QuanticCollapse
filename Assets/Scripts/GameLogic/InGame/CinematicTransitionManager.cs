using System.Collections;
using UnityEngine;

namespace QuanticCollapse
{
    public class CinematicTransitionManager : MonoBehaviour
    {
        [HideInInspector] public bool OnTransition;

        [SerializeField] private InitialSceneGeneralCanvas canvas;
        [SerializeField] private StarshipVisuals starship;
        [SerializeField] private BlackCircleTransition blackCircleTransition;

        private CameraTransitionEffect _cameraLogic;

        public IEnumerator CinematicTransition()
        {
            OnTransition = true;
            canvas.CanvasEngageTrigger(true);
            starship.EngageOnMissionAnimation();
            _cameraLogic.CameraOnEngageEffect();
            blackCircleTransition.TriggerCircleToClose();
            yield return new WaitForSeconds(2f);
            OnTransition = false;
        }

        private void Awake()
        {
            _cameraLogic = Camera.main.GetComponent<CameraTransitionEffect>();
        }
    }
}