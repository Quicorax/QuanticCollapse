using UnityEngine;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class StarshipVisuals : MonoBehaviour
{
    const string StarshipModelAdrsKey = "StarshipPrefab_";

    const string DynamicTransition = "_DynamicTransition";

    const string OriginalPrimaryColor = "_OriginalPrimaryColor";
    const string OriginalSecondaryColor = "_OriginalSecondaryColor";
    const string OriginalSignatureColor = "_OriginalSignatureColor";

    const string PrimaryColor = "_PrimaryColor";
    const string SecondaryColor = "_SecondaryColor";
    const string SignatureColor = "_SignatureColor";

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    private MasterSceneManager _masterSceneManager;

    //Animation CTRL
    private bool _onAnimationTransition;
    private Tween idleTweenRot;
    private Tween idleTweenMov;
    [SerializeField] private float floatingDispersion;

    //Skin Mat CTRL
    private bool _onSkinTransition;
    private Material _material;
    private ParticleSystem[] _thrusterParticles;
    private DeSeializedStarshipColors _equipedSkin;
    [SerializeField] private DeSeializedStarshipColors[] initialColorSkins;

    //Skin Geo CTRL
    private StarshipGeoModel _equipedGeo;
    private GameObject _instancedStarshipGeo;
    [SerializeField] private StarshipGeoModel initialGeo;

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
        if(_masterSceneManager.Inventory.GetEquipedStarshipColors() == null)
        {
            for (int i = 0; i < initialColorSkins.Length; i++)
                _masterSceneManager.Inventory.AddElementToUnlockedSkins(initialColorSkins[i]);

            _masterSceneManager.Inventory.SetEquipedStarshipColors(initialColorSkins[Random.Range(0, initialColorSkins.Length)]);
        }

        if(_masterSceneManager.Inventory.GetEquipedStarshipGeoIndex() == null)
        {
            _masterSceneManager.Inventory.AddElementToUnlockedGeo(initialGeo);
            _masterSceneManager.Inventory.SetEquipedStarshipGeoIndex(initialGeo);
        }

        SetStarshipGeo(_masterSceneManager.Inventory.GetEquipedStarshipGeoIndex());
        SetOnInitialPositionAnimation();
    }

    public void SetMasterReference(MasterSceneManager masterReference) => _masterSceneManager = masterReference;

    public void SetOnInitialPositionAnimation() => transform.DOMoveZ(-10, 3).From().SetEase(Ease.OutBack).OnComplete(() => IdleAnimation());
    public void IdleAnimation()
    {
        if (_onAnimationTransition)
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
        _onAnimationTransition = true;
        DeleteIdleTweens();

        transform.DOLocalRotate(Vector3.zero, 1);
        transform.DOMoveZ(transform.position.z + 70, 2f).SetEase(Ease.InBack);
        transform.DOScale(0,2f).SetEase(Ease.InExpo);
    }


    public void SetStarshipGeo(StarshipGeoModel geo)
    {
        if (_onSkinTransition || _equipedGeo == geo)
            return;
        _onSkinTransition = true;

        _masterSceneManager.Inventory.SetEquipedStarshipGeoIndex(geo);
        _equipedGeo = geo;

        if(_instancedStarshipGeo != null)
            Addressables.ReleaseInstance(_instancedStarshipGeo);

        string StarshipAdrKey = StarshipModelAdrsKey + geo.StarshipName;
        
        Addressables.LoadAssetAsync<GameObject>(StarshipAdrKey).Completed += handle =>
        {
            _instancedStarshipGeo = Addressables.InstantiateAsync(StarshipAdrKey, transform).Result;
            _instancedStarshipGeo.name = StarshipAdrKey;

            _material = _instancedStarshipGeo.GetComponent<MeshRenderer>().material;
            _thrusterParticles = _instancedStarshipGeo.GetComponentsInChildren<ParticleSystem>();

            _onSkinTransition = false;
            SetStarshipColors(_masterSceneManager.Inventory.GetEquipedStarshipColors());
        };
    }

    public void SetStarshipColors(DeSeializedStarshipColors skin)
    {
        if (_onSkinTransition)
            return;
        _onSkinTransition = true;

        _masterSceneManager.Inventory.SetEquipedStarshipColors(skin);

        _material.SetColor(PrimaryColor, skin.SkinColors[0]);
        _material.SetColor(SecondaryColor, skin.SkinColors[1]);
        _material.SetColor(SignatureColor, skin.SkinColors[2]);

        transform.DOPunchScale(Vector3.one * -0.1f, 0.3f, 10, 1).SetEase(Ease.InQuint);

        foreach (var particle in _thrusterParticles)
            particle.startColor = skin.SkinColors[2];

        DOTween.To(() => _material.GetFloat(DynamicTransition), x => _material.SetFloat(DynamicTransition, x), -1, 0.7f).OnComplete(() =>
        {
            _onSkinTransition = false;

            _equipedSkin = skin;

            _material.SetColor(OriginalPrimaryColor, _equipedSkin.SkinColors[0]);
            _material.SetColor(OriginalSecondaryColor, _equipedSkin.SkinColors[1]);
            _material.SetColor(OriginalSignatureColor, _equipedSkin.SkinColors[2]);

            _material.SetFloat(DynamicTransition, 1);

            _masterSceneManager.SaveAll();
        });
    }
}
