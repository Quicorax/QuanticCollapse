using UnityEngine;
using DG.Tweening;

public class StarshipView : MonoBehaviour
{
    [SerializeField] private float floatingDispersion;

    private Material _material;

    [SerializeField] private ParticleSystem[] ThrusterParticles;

    public ColorPack InitialColorPack;

    private ColorPack _starshipColor;
    public ColorPack StarshipColors 
    {
        get => _starshipColor;
        set
        {
            _starshipColor = value;
            ApplyStarshipColors();
        }
    }

    Tween idleTweenRot;
    Tween idleTweenMov;
    bool onTransition;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
    }
    private void Start()
    {
        SetStarshipColors(InitialColorPack);
        SetOnInitialPositionAnimation();
    }

    public void SetOnInitialPositionAnimation()
    {
        transform.DOMoveZ(-10, 3).From().SetEase(Ease.OutBack).OnComplete(() => IdleAnimation());
    }

    public void IdleAnimation()
    {
        if (onTransition)
            return;

        float rngY = Random.Range(-floatingDispersion, floatingDispersion);
        float rngX = Random.Range(-floatingDispersion, floatingDispersion);
        float rngZ = Random.Range(-floatingDispersion, floatingDispersion);

        idleTweenRot = transform.DOLocalRotate(Vector3.forward * (rngX > 0 ? 5f : -5f), 3f);
        idleTweenMov = transform.DOMove(new Vector3(rngX, rngY, rngZ), 3f).SetEase(Ease.InOutSine).OnComplete(() => IdleAnimation());
    }
    void DeleteIdleTweens()
    {
        DOTween.Kill(idleTweenRot);
        DOTween.Kill(idleTweenMov);
    }
    public void EngageOnMissionAnimation()
    {
        onTransition = true;
        DeleteIdleTweens();

        transform.DOLocalRotate(Vector3.zero, 1);
        transform.DOMoveZ(transform.position.z + 70, 2f).SetEase(Ease.InBack);
        transform.DOScale(0,2f).SetEase(Ease.InExpo);
    }
    public void SetStarshipColors(ColorPack colors) => StarshipColors = colors;

    void ApplyStarshipColors()
    {
        if (_starshipColor == null)
            return;

        _material.SetColor("_BaseColor", StarshipColors.BaseColor);
        _material.SetColor("_SecondaryColor", StarshipColors.SecondaryColor);
        _material.SetColor("_SignatureColor", StarshipColors.SignatureColor);

        foreach (var particle in ThrusterParticles)
        {
            particle.startColor = StarshipColors.SignatureColor;
        }
    }
}
