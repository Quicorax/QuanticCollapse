using UnityEngine;
using DG.Tweening;

public class StarshipVisuals : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;

    [SerializeField] private float floatingDispersion;
    private MasterSceneManager _masterSceneManager;

    private Material _material;

    private ParticleSystem[] _thrusterParticles;

    //[HideInInspector] public ColorPackScriptable initialColor;
    [HideInInspector] public GameObject currentShip;

    //private ColorPackScriptable _starshipColor;

    public GameObject[] shipModels;

    //public ColorPackScriptable StarshipColors 
    //{
    //    get => _starshipColor;
    //    set
    //    {
    //        _starshipColor = value;
    //        ApplyStarshipColors();
    //    }
    //}

    Tween idleTweenRot;
    Tween idleTweenMov;
    bool onTransition;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    private void Start()
    {
        //if(_masterSceneManager.Inventory.GetStarshipColors() == null)
        //    _masterSceneManager.Inventory.SetStarshipColors(initialColor);

        SetStarshipPrefab(0);

        SetOnInitialPositionAnimation();
    }
    public void SetMasterReference(MasterSceneManager masterReference) => _masterSceneManager = masterReference;

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
    //public void SetStarshipColors(ColorPackScriptable colors)
    //{
    //    _masterSceneManager.Inventory.SaveFiles.Configuration.StarshipEquipedColors = colors;
    //    StarshipColors = colors;
    //}
    //void ApplyStarshipColors()
    //{
    //    if (_starshipColor == null)
    //        return;
    //
    //    _material.SetColor("_BaseColor", StarshipColors.BaseColor);
    //    _material.SetColor("_SecondaryColor", StarshipColors.SecondaryColor);
    //    _material.SetColor("_SignatureColor", StarshipColors.SignatureColor);
    //
    //    foreach (var particle in _thrusterParticles)
    //        particle.startColor = StarshipColors.SignatureColor;
    //}
    //
    public void SetStarshipPrefab(int index)
    {
        if (currentShip != null)
            Destroy(currentShip);
    
        currentShip = Instantiate(shipModels[index], transform);
    
        _material = currentShip.GetComponent<MeshRenderer>().material;
        _thrusterParticles = currentShip.GetComponentsInChildren<ParticleSystem>();
    
        //SetStarshipColors(_masterSceneManager.Inventory.GetStarshipColors());
    }
}
