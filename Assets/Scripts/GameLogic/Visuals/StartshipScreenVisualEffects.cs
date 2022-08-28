using System.Collections;
using UnityEngine;
using DG.Tweening;

public class StartshipScreenVisualEffects : MonoBehaviour
{
    const string AimSightColor = "_AimSightColor";
    const string Aim_Center_Y = "_Aim_Center_Y";
    const string GeneralAlpha = "_GeneralAlpha";
    const string ScreenFresnelColor = "_Color"; 
    const string SpaceGeneralColor = "_SpaceGeneralColor";

    [SerializeField] private LevelInjectedEventBus _LevelInjected;
    [SerializeField] private GenericEventBus _PlayerHitEventBus;
    [SerializeField] private ExternalBoosterScreenEffectEventBus _ExternalBoosterScreenEffects;

    [SerializeField] private Material screenShader;

    [SerializeField] private float timeBetweenAimSignal = 0.2f;
    [SerializeField] private int aimScopeSignalRepeat;

    [SerializeField] private float finalScopeYPosition = -4.93f;
    [SerializeField] private float generalAlphaFinalAmount = 0.63f;

    [SerializeField] private Color originalBaseColor;


    private void Awake()
    {
        _PlayerHitEventBus.Event += Hit;
        _ExternalBoosterScreenEffects.Event += ExternalBoosterScreenEffects;

        _LevelInjected.Event += SetLevelColorData;
    }
    private void OnDisable()
    {
        _PlayerHitEventBus.Event -= Hit;
        _ExternalBoosterScreenEffects.Event -= ExternalBoosterScreenEffects;

        _LevelInjected.Event -= SetLevelColorData;

        screenShader.SetFloat(Aim_Center_Y, 0);
        screenShader.SetFloat(GeneralAlpha, 2);
        screenShader.SetColor(ScreenFresnelColor, originalBaseColor);
    }
    void Start()
    {
        InitialEffect();
    }

    void SetLevelColorData(LevelModel data)
    {
        string[] colorString = data.Color.Split("-");

        Color color = new Color(float.Parse(colorString[0]) / 100, float.Parse(colorString[1]) / 100, float.Parse(colorString[2]) / 100);

        screenShader.SetColor(SpaceGeneralColor, color);
    }
    public void InitialEffect()
    {
        screenShader.DOFloat(finalScopeYPosition, Aim_Center_Y, 2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            StartCoroutine(LockTarget());
        });

        screenShader.DOFloat(generalAlphaFinalAmount, GeneralAlpha, 2f).SetEase(Ease.InOutBack);
    }

    IEnumerator LockTarget()
    {
        for (int i = 0; i < aimScopeSignalRepeat; i++)
        {
            screenShader.SetColor(AimSightColor, Color.red);
            yield return new WaitForSeconds(timeBetweenAimSignal);
            screenShader.SetColor(AimSightColor, Color.white);
            yield return new WaitForSeconds(timeBetweenAimSignal);
        }
    }

    public void Hit()
    {
        screenShader.DOColor(Color.red, 1f).OnComplete(() =>
        {
            screenShader.DOColor(originalBaseColor, ScreenFresnelColor, 0.5f);
        });
    }
    public void ExternalBoosterScreenEffects(string externalBoosterName)
    {
        if(externalBoosterName == "FirstAidKit")
        {
            screenShader.DOFloat(1, GeneralAlpha, 1f);
            screenShader.DOColor(Color.green, 1f).OnComplete(() =>
            {
                screenShader.DOFloat(generalAlphaFinalAmount, GeneralAlpha, 0.5f);
                screenShader.DOColor(originalBaseColor, ScreenFresnelColor, 0.5f);
            });
        }
        else if (externalBoosterName == "EasyTrigger")
        {
            screenShader.DOFloat(1, GeneralAlpha, 1f);
            screenShader.DOColor(new Color(1, 0, 1), 1f).OnComplete(() =>
            {
                screenShader.DOFloat(generalAlphaFinalAmount, GeneralAlpha, 0.5f);
                screenShader.DOColor(originalBaseColor, ScreenFresnelColor, 0.5f);
            });
        }
        else if(externalBoosterName == "DeAthomizer")
        {
            screenShader.DOFloat(1, GeneralAlpha, 1f);
            screenShader.DOColor(Color.yellow, 1f).OnComplete(() =>
            {
                screenShader.DOFloat(generalAlphaFinalAmount, GeneralAlpha, 0.5f);
                screenShader.DOColor(originalBaseColor, ScreenFresnelColor, 0.5f);
            });
        }
    }
}
