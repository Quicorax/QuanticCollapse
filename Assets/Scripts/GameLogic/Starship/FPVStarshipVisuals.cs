using UnityEngine;

namespace QuanticCollapse
{
    public class FPVStarshipVisuals : MonoBehaviour
    {
        [SerializeField] private Material fpvMaterial;
        [SerializeField] private StartshipScreenVisualEffects screenVisuals;

        private void Start()
        {
            SetStarshipGeo(PlayerPrefs.GetString("EquipedStarshipModel"));

            var colors = ServiceLocator.GetService<StarshipVisualsService>()
                .GetColorPackByName(PlayerPrefs.GetString("EquipedStarshipColors"));

            SetColors(colors);
            screenVisuals.SetSignatureColor(colors.SkinColors[2]);
        }

        private void SetStarshipGeo(string starshipModelName)
        {
            var addressableKey = "FPV_Starship_" + starshipModelName;

            ServiceLocator.GetService<AddressablesService>()
                .LoadAddrsOfComponent<GameObject>(addressableKey, transform, null);
        }

        private void SetColors(DeSeializedStarshipColors skin)
        {
            fpvMaterial.SetColor("_PrimaryColor", skin.SkinColors[0]);
            fpvMaterial.SetColor("_SecondaryColor", skin.SkinColors[1]);
            fpvMaterial.SetColor("_SignatureColor", skin.SkinColors[2]);
        }
    }
}