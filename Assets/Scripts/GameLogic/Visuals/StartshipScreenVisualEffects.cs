using System.Collections;
using UnityEngine;
using DG.Tweening;

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

        _screenShader.SetFloat(Constants.AimCenterY, 0);
        _screenShader.SetFloat(Constants.GeneralAlpha, 2);
        _screenShader.SetColor(Constants.ScreenFresnelColor, originalBaseColor);
    }
    void Start()
    {
        InitialEffect();
    }

    public void SetSignatureColor(Color color)
    {
        originalBaseColor = color;
        _screenShader.SetColor(Constants.ScreenFresnelColor, originalBaseColor);
    }

    void SetLevelColorData(LevelModel data)
    {
        string[] colorString = data.Color.Split(Constants.MiddleBar);

        Color color = new(float.Parse(colorString[0]) / 100, float.Parse(colorString[1]) / 100, float.Parse(colorString[2]) / 100);

        externalSpaceShader.SetColor(Constants.SpaceGeneralColor, color);
    }
    public void InitialEffect()
    {
        _screenShader.DOFloat(finalScopeYPosition, Constants.AimCenterY, 2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            StartCoroutine(LockTarget());
        });

        _screenShader.DOFloat(generalAlphaFinalAmount, Constants.GeneralAlpha, 2f).SetEase(Ease.InOutBack);
    }

    IEnumerator LockTarget()
    {
        for (int i = 0; i < aimScopeSignalRepeat; i++)
        {
            _screenShader.SetColor(Constants.AimSightColor, Color.red);
            yield return new WaitForSeconds(timeBetweenAimSignal);
            _screenShader.SetColor(Constants.AimSightColor, Color.white);
            yield return new WaitForSeconds(timeBetweenAimSignal);
        }
    }

    public void Hit()
    {
        Handheld.Vibrate();
        _screenShader.DOColor(Color.red, 1f).OnComplete(() =>
        {
            _screenShader.DOColor(originalBaseColor, Constants.ScreenFresnelColor, 0.5f);
        });
    }
    public void ExternalBoosterScreenEffects(ResourcesType externalBoosterType)
    {
        _screenShader.DOFloat(1, Constants.GeneralAlpha, 1f);
        _screenShader.DOColor(GetExternalBoosterColor(externalBoosterType), 1f).OnComplete(() =>
        {
            _screenShader.DOFloat(generalAlphaFinalAmount, Constants.GeneralAlpha, 0.5f);
            _screenShader.DOColor(originalBaseColor, Constants.ScreenFresnelColor, 0.5f);
        });
    }
    private Color GetExternalBoosterColor(ResourcesType externalBoosterType)
    {
        Color color;

        switch (externalBoosterType)
        {
            default:
            case ResourcesType.FirstAidKit:
                color = Color.green;
                break;
            case ResourcesType.EasyTrigger:
                color = new Color(1, 0, 1);
                break;
            case ResourcesType.DeAthomizer:
                color = Color.yellow;
                break;
        }
        return color;
    }
}
