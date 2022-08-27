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
    [SerializeField] private GenericEventBus _playerHitEventBus;

    [SerializeField] private Material screenShader;

    [SerializeField] private float timeBetweenAimSignal = 0.2f;
    [SerializeField] private int aimScopeSignalRepeat;

    [SerializeField] private float finalScopeYPosition = -4.93f;
    [SerializeField] private float generalAlphaFinalAmount = 0.63f;

    [SerializeField] private Color originalBaseColor;


    private void Awake()
    {
        _playerHitEventBus.Event += Hit;
        _LevelInjected.Event += SetLevelColorData;
    }
    private void OnDisable()
    {
        _playerHitEventBus.Event -= Hit;
        _LevelInjected.Event -= SetLevelColorData;

        screenShader.SetFloat(Aim_Center_Y, 0);
        screenShader.SetFloat(GeneralAlpha, 2);
        screenShader.SetColor(ScreenFresnelColor, originalBaseColor);
    }
    void Start()
    {
        InitialEffect();
    }

    void SetLevelColorData(LevelGridData data) => screenShader.SetColor(SpaceGeneralColor, data.SpaceGeneralColor);

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
    public void Healed()
    {
        screenShader.DOFloat(1, GeneralAlpha, 1f);
        screenShader.DOColor(Color.green, 1f).OnComplete(() =>
        {
            screenShader.DOFloat(generalAlphaFinalAmount, GeneralAlpha, 0.5f);
            screenShader.DOColor(originalBaseColor, ScreenFresnelColor, 0.5f);
        });
    }
}
