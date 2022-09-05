using UnityEngine;
using UnityEngine.AddressableAssets;

public class FPVStarshipVisuals : MonoBehaviour
{
    const string PrimaryColor = "_PrimaryColor";
    const string SecondaryColor = "_SecondaryColor";
    const string SignatureColor = "_SignatureColor";

    const string FPVGeoArdKey = "FPV_Starship_";

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    private MasterSceneManager _masterSceneManager;

    [SerializeField] private Material fpvMaterial;
    [SerializeField] private StartshipScreenVisualEffects screenVisuals;
    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    void Start()
    {
        SetStarshipGeo(_masterSceneManager.Inventory.GetEquipedStarshipGeo());

        DeSeializedStarshipColors colors = _masterSceneManager.Inventory.GetEquipedStarshipColors();
        SetColors(colors);
        screenVisuals.SetSignatureColor(colors.SkinColors[2]);
    }
    public void SetMasterReference(MasterSceneManager masterReference) => _masterSceneManager = masterReference;

    void SetStarshipGeo(StarshipGeoModel geo)
    {
        string adressableKey = FPVGeoArdKey + geo.StarshipName;
        Addressables.LoadAssetAsync<GameObject>(adressableKey).Completed += handle =>
        {
            GameObject element = Addressables.InstantiateAsync(adressableKey, transform).Result;
            element.gameObject.name = adressableKey;
        };
    }
    void SetColors(DeSeializedStarshipColors skin)
    {
        fpvMaterial.SetColor(PrimaryColor, skin.SkinColors[0]);
        fpvMaterial.SetColor(SecondaryColor, skin.SkinColors[1]);
        fpvMaterial.SetColor(SignatureColor, skin.SkinColors[2]);
    }
}
