using UnityEngine;
using DG.Tweening;
public class StarshipView : MonoBehaviour
{
    [SerializeField] private float floatingDispersion;

    private bool _transitioning;
    private Vector3 _initialPosition;
    private Material _material;

    [SerializeField] private ParticleSystem[] ThrusterParticles;

    public ColorPack StarshipColors;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
    }
    private void Start()
    {
        _initialPosition = transform.position;
        
        InitFloatation();
        SetStarshipColors();
    }
    void InitFloatation()
    {
        if (_transitioning)
            return;

        float rngY = Random.Range(-floatingDispersion, floatingDispersion);
        float rngX = Random.Range(-floatingDispersion, floatingDispersion);
        float rngZ = Random.Range(-floatingDispersion, floatingDispersion);

        transform.DOLocalRotate(Vector3.forward * (rngX > 0 ? 5f : -5f), 2f);
        transform.DOMove(_initialPosition + new Vector3(rngX, rngY, rngZ), 2f).SetEase(Ease.InOutSine).OnComplete(() => InitFloatation());
    }

    public void TriggerTransitionAnimation()
    {
        _transitioning = true;

        transform.DOLocalRotate(Vector3.zero, 1);
        transform.DOMoveZ(transform.position.z + 70, 2f).SetEase(Ease.InBack);
        transform.DOScale(0,2f).SetEase(Ease.InExpo);
    }
    void SetStarshipColors()
    {
        _material.SetColor("_BaseColor", StarshipColors.BaseColor);
        _material.SetColor("_SecondaryColor", StarshipColors.SecondaryColor);
        _material.SetColor("_SignatureColor", StarshipColors.SignatureColor);

        foreach (var particle in ThrusterParticles)
        {
            particle.startColor = StarshipColors.SignatureColor;
        }
    }
}
