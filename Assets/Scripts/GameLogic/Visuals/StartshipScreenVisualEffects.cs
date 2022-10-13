using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace QuanticCollapse
{
    public class StartshipScreenVisualEffects : MonoBehaviour
    {
        private Material _screenShader;

        [SerializeField] private LevelInjectedEventBus _LevelInjected;
        [SerializeField] private GenericEventBus _PlayerHitEventBus;
        [SerializeField] private ExternalBoosterScreenEffectEventBus _ExternalBoosterScreenEffects;

        [SerializeField] private float timeBetweenAimSignal = 0.2f;
        [SerializeField] private int aimScopeSignalRepeat;

        [SerializeField] private float finalScopeYPosition = -4.93f;
        [SerializeField] private float generalAlphaFinalAmount = 0.63f;

        [SerializeField] private Color originalBaseColor;

        [SerializeField] private Material externalSpaceShader;


        private void Awake()
        {
            _PlayerHitEventBus.Event += Hit;
            _ExternalBoosterScreenEffects.Event += ExternalBoosterScreenEffects;

            _LevelInjected.Event += SetLevelColorData;

            _screenShader = GetComponent<MeshRenderer>().material;
        }
        private void OnDisable()
        {
            _PlayerHitEventBus.Event -= Hit;
            _ExternalBoosterScreenEffects.Event -= ExternalBoosterScreenEffects;

            _LevelInjected.Event -= SetLevelColorData;

            _screenShader.SetFloat("_Aim_Center_Y", 0);
            _screenShader.SetFloat("_GeneralAlpha", 2);
            _screenShader.SetColor("_Color", originalBaseColor);
        }
        void Start()
        {
            InitialEffect();
        }

        public void SetSignatureColor(Color color)
        {
            originalBaseColor = color;
            _screenShader.SetColor("_Color", originalBaseColor);
        }

        void SetLevelColorData(LevelModel data)
        {
            ColorUtility.TryParseHtmlString(data.Color, out Color primaryColor);
            externalSpaceShader.SetColor("_SpaceGeneralColor", primaryColor);
        }
        public void InitialEffect()
        {
            _screenShader.DOFloat(finalScopeYPosition, "_Aim_Center_Y", 2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                StartCoroutine(LockTarget());
            });

            _screenShader.DOFloat(generalAlphaFinalAmount, "_GeneralAlpha", 2f).SetEase(Ease.InOutBack);
        }

        IEnumerator LockTarget()
        {
            for (int i = 0; i < aimScopeSignalRepeat; i++)
            {
                _screenShader.SetColor("_AimSightColor", Color.red);
                yield return new WaitForSeconds(timeBetweenAimSignal);
                _screenShader.SetColor("_AimSightColor", Color.white);
                yield return new WaitForSeconds(timeBetweenAimSignal);
            }
        }

        public void Hit()
        {
            Handheld.Vibrate();
            _screenShader.DOColor(Color.red, 1f).OnComplete(() =>
            {
                _screenShader.DOColor(originalBaseColor, "_Color", 0.5f);
            });
        }
        public void ExternalBoosterScreenEffects(string externalBoosterId)
        {
            _screenShader.DOFloat(1, "_GeneralAlpha", 1f);
            _screenShader.DOColor(GetExternalBoosterColor(externalBoosterId), 1f).OnComplete(() =>
            {
                _screenShader.DOFloat(generalAlphaFinalAmount, "_GeneralAlpha", 0.5f);
                _screenShader.DOColor(originalBaseColor, "_Color", 0.5f);
            });
        }
        private Color GetExternalBoosterColor(string externalBoosterId)
        {
            Color color;

            switch (externalBoosterId)
            {
                default:
                case "FirstAidKit":
                    color = Color.green;
                    break;
                case "EasyTrigger":
                    color = new Color(1, 0, 1);
                    break;
                case "DeAthomizer":
                    color = Color.yellow;
                    break;
            }
            return color;
        }
    }
}