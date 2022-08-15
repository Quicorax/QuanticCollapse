using System.Collections;
using UnityEngine;
using DG.Tweening;

public class StartshipScreenVisualEvents : MonoBehaviour
{
    const string AimSightColor = "_AimSightColor";
    const string Aim_Center_Y = "_Aim_Center_Y";
    const string GeneralAlpha = "_GeneralAlpha";
    const string BaseColor = "_Color";

    [SerializeField] Material screenShader;

    [SerializeField] float timeBetweenAimSignal = 0.2f;
    [SerializeField] int aimScopeSignalRepeat;

    [SerializeField] float finalScopeYPosition = -4.93f;
    [SerializeField] float generalAlphaFinalAmount = 0.63f;

    [SerializeField] Color originalBaseColor;

    [SerializeField] private GenericEventBus _playerHitEventBus;

    private void Awake()
    {
        _playerHitEventBus.Event += Hit;
    }
    void Start()
    {
        InitialEffect();
    }
    private void OnDisable()
    {
        _playerHitEventBus.Event -= Hit;

        screenShader.SetFloat(Aim_Center_Y, 0);
        screenShader.SetFloat(GeneralAlpha, 2);
        screenShader.SetColor(BaseColor, originalBaseColor);
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
            screenShader.DOColor(originalBaseColor, BaseColor, 0.5f);
        });
    }
    public void Healed()
    {
        screenShader.DOFloat(1, GeneralAlpha, 1f);
        screenShader.DOColor(Color.green, 1f).OnComplete(() =>
        {
            screenShader.DOFloat(generalAlphaFinalAmount, GeneralAlpha, 0.5f);
            screenShader.DOColor(originalBaseColor, BaseColor, 0.5f);
        });
    }
}
