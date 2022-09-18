using UnityEngine;

public class FPVStarshipVisuals : MonoBehaviour
{
    [SerializeField] private Material fpvMaterial;
    [SerializeField] private StartshipScreenVisualEffects screenVisuals;

    void Start()
    {
        SetStarshipGeo(PlayerPrefs.GetString(Constants.EquipedStarshipModel));

        DeSeializedStarshipColors colors = ServiceLocator.GetService<StarshipVisualsService>()
            .GetColorPackByName(PlayerPrefs.GetString(Constants.EquipedStarshipColors));

        SetColors(colors);
        screenVisuals.SetSignatureColor(colors.SkinColors[2]);
    }

    void SetStarshipGeo(string starshipModelName)
    {
        string adressableKey = Constants.StarshipFPVModel + starshipModelName;

        ServiceLocator.GetService<AddressablesService>().SpawnAddressable<GameObject>(adressableKey, transform, null);
    }
    void SetColors(DeSeializedStarshipColors skin)
    {
        fpvMaterial.SetColor(Constants.PrimaryColor, skin.SkinColors[0]);
        fpvMaterial.SetColor(Constants.SecondaryColor, skin.SkinColors[1]);
        fpvMaterial.SetColor(Constants.SignatureColor, skin.SkinColors[2]);
    }
}
