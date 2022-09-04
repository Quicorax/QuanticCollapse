using UnityEngine;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class StarshipVisuals : MonoBehaviour
{
    const string StarshipModelAdrsKeyWithNoIndex = "StarshipPrefab_";

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
    private DeSeializedStarshipColors equipedSkin;
    [SerializeField] private DeSeializedStarshipColors initialColorSkin;

    //Skin Geo CTRL


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
            _masterSceneManager.Inventory.AddElementToUnlockedSkins(initialColorSkin);
            _masterSceneManager.Inventory.SetEquipedStarshipColors(initialColorSkin);
            _masterSceneManager.SaveAll();
        }

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

            equipedSkin = skin;

            _material.SetColor(OriginalPrimaryColor, equipedSkin.SkinColors[0]);
            _material.SetColor(OriginalSecondaryColor, equipedSkin.SkinColors[1]);
            _material.SetColor(OriginalSignatureColor, equipedSkin.SkinColors[2]);

            _material.SetFloat(DynamicTransition, 1);
        });
    }
    public void SetStarshipPrefab(int index)
    {
        string StarshipAdrKey = StarshipModelAdrsKeyWithNoIndex + index;
        Addressables.LoadAssetAsync<GameObject>(StarshipAdrKey).Completed += handle =>
        {
            GameObject element = Addressables.InstantiateAsync(StarshipAdrKey, transform).Result;
            element.name = StarshipAdrKey;

            _material = element.GetComponent<MeshRenderer>().material;
            _thrusterParticles = element.GetComponentsInChildren<ParticleSystem>();
    
            SetStarshipColors(_masterSceneManager.Inventory.GetEquipedStarshipColors());
        };
    }
}
