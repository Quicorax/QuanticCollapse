using UnityEngine;
using UnityEngine.AddressableAssets;

public class FPVStarshipVisuals : MonoBehaviour
{
    [SerializeField] private Material fpvMaterial;
    [SerializeField] private StartshipScreenVisualEffects screenVisuals;

    private StarshipVisualsService _starshipColors;

    private void Awake()
    {
        _starshipColors = ServiceLocator.GetService<StarshipVisualsService>();
    }
    void Start()
    {
        SetStarshipGeo(PlayerPrefs.GetString(Constants.EquipedStarshipModel));

        DeSeializedStarshipColors colors = _starshipColors.GetColorPackByName(PlayerPrefs.GetString(Constants.EquipedStarshipColors));
        SetColors(colors);
        screenVisuals.SetSignatureColor(colors.SkinColors[2]);
    }

    void SetStarshipGeo(string starshipModelName)
    {
        string adressableKey = Constants.StarshipFPVModel + starshipModelName;
        Addressables.LoadAssetAsync<GameObject>(adressableKey).Completed += handle =>
        {
            GameObject element = Addressables.InstantiateAsync(adressableKey, transform).Result;
            element.gameObject.name = adressableKey;
        };
    }
    void SetColors(DeSeializedStarshipColors skin)
    {
        fpvMaterial.SetColor(Constants.PrimaryColor, skin.SkinColors[0]);
        fpvMaterial.SetColor(Constants.SecondaryColor, skin.SkinColors[1]);
        fpvMaterial.SetColor(Constants.SignatureColor, skin.SkinColors[2]);
    }
}
