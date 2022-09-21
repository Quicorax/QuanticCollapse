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

    //Animation CTRL
    private bool _onAnimationTransition;
    private Tween idleTweenRot;
    private Tween idleTweenMov;
    [SerializeField] private float floatingDispersion;

    //Skin Mat CTRL
    private bool _onSkinTransition;
    private Material _material;
    private ParticleSystem[] _thrusterParticles;

    //Skin Geo CTRL
    private string _equipedStarshipName;
    private GameObject _instancedStarshipGeo;

    private StarshipVisualsService _starshipColors;

    private void Awake()
    {
        _starshipColors = ServiceLocator.GetService<StarshipVisualsService>();
    }
    private void Start()
    {
        SetStarshipGeo(PlayerPrefs.GetString(Constants.EquipedStarshipModel));
        SetOnInitialPositionAnimation();
    }

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

        transform.DOScale(0,2f).SetEase(Ease.InExpo);
        transform.DOLocalRotate(Vector3.zero, 1);
        transform.DOMoveZ(transform.position.z + 70, 1.9f).SetEase(Ease.InBack).OnComplete(() => 
        {
            foreach (var item in _thrusterParticles) 
                item.gameObject.SetActive(false);
        });
    }


    public void SetStarshipGeo(string starshipName)
    {
        if (_onSkinTransition || _equipedStarshipName == starshipName)
            return;
        _onSkinTransition = true;

        transform.DOPunchScale(Vector3.one * -0.5f, 0.3f, 10, 1).SetEase(Ease.InQuint);

        PlayerPrefs.SetString(Constants.EquipedStarshipModel, starshipName);

        _equipedStarshipName = starshipName;

        if(_instancedStarshipGeo != null)
            Addressables.ReleaseInstance(_instancedStarshipGeo);

        string StarshipAdrKey = StarshipModelAdrsKey + starshipName;
        
        Addressables.LoadAssetAsync<GameObject>(StarshipAdrKey).Completed += handle =>
        {
            _instancedStarshipGeo = Addressables.InstantiateAsync(StarshipAdrKey, transform).Result;
            _instancedStarshipGeo.name = StarshipAdrKey;

            _material = _instancedStarshipGeo.GetComponent<MeshRenderer>().material;
            _thrusterParticles = _instancedStarshipGeo.GetComponentsInChildren<ParticleSystem>();

            _onSkinTransition = false;
            SetStarshipColors(_starshipColors.GetColorPackByName(PlayerPrefs.GetString(Constants.EquipedStarshipColors)), false);
        };
    }

    public void SetStarshipColors(DeSeializedStarshipColors colorPack, bool isVisual = true)
    {
        if (_onSkinTransition)
            return;
        _onSkinTransition = true;

        PlayerPrefs.SetString(Constants.EquipedStarshipColors, colorPack.SkinName);

        _material.SetColor(PrimaryColor, colorPack.SkinColors[0]);
        _material.SetColor(SecondaryColor, colorPack.SkinColors[1]);
        _material.SetColor(SignatureColor, colorPack.SkinColors[2]);
       

        if(isVisual)
            transform.DOPunchScale(Vector3.one * -0.1f, 0.3f, 10, 1).SetEase(Ease.InQuint);

        foreach (var particle in _thrusterParticles)
            particle.startColor = colorPack.SkinColors[2];

        DOTween.To(() => _material.GetFloat(DynamicTransition), x => _material.SetFloat(DynamicTransition, x), -1, 0.7f).OnComplete(() =>
        {
            _onSkinTransition = false;

            _material.SetColor(OriginalPrimaryColor, colorPack.SkinColors[0]);
            _material.SetColor(OriginalSecondaryColor, colorPack.SkinColors[1]);
            _material.SetColor(OriginalSignatureColor, colorPack.SkinColors[2]);

            _material.SetFloat(DynamicTransition, 1);
        });
    }
}
