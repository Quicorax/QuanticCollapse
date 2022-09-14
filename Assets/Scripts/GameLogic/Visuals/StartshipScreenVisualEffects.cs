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

        _screenShader.SetFloat(Aim_Center_Y, 0);
        _screenShader.SetFloat(GeneralAlpha, 2);
        _screenShader.SetColor(ScreenFresnelColor, originalBaseColor);
    }
    void Start()
    {
        InitialEffect();
    }

    public void SetSignatureColor(Color color)
    {
        originalBaseColor = color;
        _screenShader.SetColor(ScreenFresnelColor, originalBaseColor);
    }

    void SetLevelColorData(LevelModel data)
    {
        string[] colorString = data.Color.Split("-");

        Color color = new Color(float.Parse(colorString[0]) / 100, float.Parse(colorString[1]) / 100, float.Parse(colorString[2]) / 100);

        //Used for opaque screen shader
        //_screenShader.SetColor(SpaceGeneralColor, color);

        externalSpaceShader.SetColor(SpaceGeneralColor, color);
    }
    public void InitialEffect()
    {
        _screenShader.DOFloat(finalScopeYPosition, Aim_Center_Y, 2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            StartCoroutine(LockTarget());
        });

        _screenShader.DOFloat(generalAlphaFinalAmount, GeneralAlpha, 2f).SetEase(Ease.InOutBack);
    }

    IEnumerator LockTarget()
    {
        for (int i = 0; i < aimScopeSignalRepeat; i++)
        {
            _screenShader.SetColor(AimSightColor, Color.red);
            yield return new WaitForSeconds(timeBetweenAimSignal);
            _screenShader.SetColor(AimSightColor, Color.white);
            yield return new WaitForSeconds(timeBetweenAimSignal);
        }
    }

    public void Hit()
    {
        _screenShader.DOColor(Color.red, 1f).OnComplete(() =>
        {
            _screenShader.DOColor(originalBaseColor, ScreenFresnelColor, 0.5f);
        });
    }
    public void ExternalBoosterScreenEffects(string externalBoosterName)
    {
        if(externalBoosterName == "FirstAidKit")
        {
            _screenShader.DOFloat(1, GeneralAlpha, 1f);
            _screenShader.DOColor(Color.green, 1f).OnComplete(() =>
            {
                _screenShader.DOFloat(generalAlphaFinalAmount, GeneralAlpha, 0.5f);
                _screenShader.DOColor(originalBaseColor, ScreenFresnelColor, 0.5f);
            });
        }
        else if (externalBoosterName == "EasyTrigger")
        {
            _screenShader.DOFloat(1, GeneralAlpha, 1f);
            _screenShader.DOColor(new Color(1, 0, 1), 1f).OnComplete(() =>
            {
                _screenShader.DOFloat(generalAlphaFinalAmount, GeneralAlpha, 0.5f);
                _screenShader.DOColor(originalBaseColor, ScreenFresnelColor, 0.5f);
            });
        }
        else if(externalBoosterName == "DeAthomizer")
        {
            _screenShader.DOFloat(1, GeneralAlpha, 1f);
            _screenShader.DOColor(Color.yellow, 1f).OnComplete(() =>
            {
                _screenShader.DOFloat(generalAlphaFinalAmount, GeneralAlpha, 0.5f);
                _screenShader.DOColor(originalBaseColor, ScreenFresnelColor, 0.5f);
            });
        }
    }
}
