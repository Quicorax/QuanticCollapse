using UnityEngine;
using UnityEngine.AddressableAssets;

public class FPVStarshipVisuals : MonoBehaviour
{
    const string PrimaryColor = "_PrimaryColor";
    const string SecondaryColor = "_SecondaryColor";
    const string SignatureColor = "_SignatureColor";

    const string FPVGeoArdKey = "FPV_Starship_";

    [SerializeField] private Material fpvMaterial;
    [SerializeField] private StartshipScreenVisualEffects screenVisuals;

    private StarshipColorsService _starshipColors;

    private void Awake()
    {
        _starshipColors = ServiceLocator.GetService<StarshipColorsService>();
    }
    void Start()
    {
        SetStarshipGeo(PlayerPrefs.GetString("EquipedStarshipModel"));

        DeSeializedStarshipColors colors = _starshipColors.GetColorPackByName(PlayerPrefs.GetString("EquipedStarshipColors"));
        SetColors(colors);
        screenVisuals.SetSignatureColor(colors.SkinColors[2]);
    }

    void SetStarshipGeo(string starshipModelName)
    {
        string adressableKey = FPVGeoArdKey + starshipModelName;
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
