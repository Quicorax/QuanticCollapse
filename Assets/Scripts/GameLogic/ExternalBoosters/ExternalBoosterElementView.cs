using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuanticCollapse
{
    public class ExternalBoosterElementView : MonoBehaviour
    {
        private Action<IExternalBooster> _onElementClicked;

        [SerializeField] private Image _externalBoosterImage;
        [SerializeField] private TMP_Text _externalBoosterAmount;

        public IExternalBooster SpecificBoosterLogic;
        private GameProgressionService _gameProgression;

        private GameConfigService _config;
        private AddressablesService _addressables;

        public void Awake()
        {
            _config = ServiceLocator.GetService<GameConfigService>();
            _addressables = ServiceLocator.GetService<AddressablesService>();
        }

        public void Initialize(
            IExternalBooster boosterElementLogic,
            GameProgressionService gameProgression,
            Action<IExternalBooster> elementClickedEvent)
        {
            SpecificBoosterLogic = boosterElementLogic;

            _onElementClicked = elementClickedEvent;
            _gameProgression = gameProgression;

            UpdateVisuals().ManageTaskException();
        }

        public void ExecuteBooster() => _onElementClicked?.Invoke(SpecificBoosterLogic);

        public void UpdateBoosterAmountText()
        {
            _externalBoosterAmount.text = _gameProgression.CheckElement(SpecificBoosterLogic.BoosterId).ToString();
            _externalBoosterAmount.color = GetBoosterColor(SpecificBoosterLogic.BoosterId);
        }

        private async Task UpdateVisuals()
        {
            var version = _config.AssetVersions.Find(x => x.Key == SpecificBoosterLogic.BoosterId)?.Version ?? -1;
            var sprite = await _addressables.LoadAssetVersion<Sprite>(SpecificBoosterLogic.BoosterId, version);

            _externalBoosterImage.sprite = sprite;
            UpdateBoosterAmountText();
        }

        private Color GetBoosterColor(string booster)
        {
            Color color = Color.white;
            switch (booster)
            {
                case "EasyTrigger":
                    color = Color.red;
                    break;
                case "FirstAidKit":
                    color = Color.green;
                    break;
                case "DeAthomizer":
                    color = Color.yellow;
                    break;
            }

            color.a = 0.75f;
            return color;
        }
    }
}