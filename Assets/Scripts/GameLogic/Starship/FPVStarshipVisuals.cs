using UnityEngine;

public class FPVStarshipVisuals : MonoBehaviour
{
    [SerializeField] private Material fpvMaterial;
    [SerializeField] private StartshipScreenVisualEffects screenVisuals;

    void Start()
    {
        SetStarshipGeo(PlayerPrefs.GetString("EquipedStarshipModel"));

        DeSeializedStarshipColors colors = ServiceLocator.GetService<StarshipVisualsService>()
            .GetColorPackByName(PlayerPrefs.GetString("EquipedStarshipColors"));

        SetColors(colors);
        screenVisuals.SetSignatureColor(colors.SkinColors[2]);
    }

    void SetStarshipGeo(string starshipModelName)
    {
        string adressableKey = "FPV_Starship_" + starshipModelName;

        ServiceLocator.GetService<AddressablesService>().SpawnAddressable<GameObject>(adressableKey, transform, null);
    }
    void SetColors(DeSeializedStarshipColors skin)
    {
        fpvMaterial.SetColor("_PrimaryColor", skin.SkinColors[0]);
        fpvMaterial.SetColor("_SecondaryColor", skin.SkinColors[1]);
        fpvMaterial.SetColor("_SignatureColor", skin.SkinColors[2]);
    }
}
