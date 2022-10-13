using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuanticCollapse
{
    public class ExternalBoosterElementView : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites = new();

        private Action<IExternalBooster> _onElementClicked;

        [SerializeField] private Image _externalBoosterImage;
        [SerializeField] private TMP_Text _externalBoosterAmount;

        public IExternalBooster SpecificBoosterLogic;
        private GameProgressionService _gameProgression;


        public void Initialize(IExternalBooster boosterElementLogic,
            GameProgressionService gameProgression,
            Action<IExternalBooster> elementClickedEvent)
        {
            SpecificBoosterLogic = boosterElementLogic;

            _onElementClicked = elementClickedEvent;
            _gameProgression = gameProgression;

            UpdateVisuals();
        }

        public void ExecuteBooster() => _onElementClicked?.Invoke(SpecificBoosterLogic);
        public void UpdateBoosterAmountText() => _externalBoosterAmount.text = _gameProgression.CheckElement(SpecificBoosterLogic.BoosterId).ToString();
        void UpdateVisuals()
        {
            _externalBoosterImage.sprite = _sprites.Find(sprite => sprite.name == SpecificBoosterLogic.BoosterId.ToString());
            UpdateBoosterAmountText();
        }
    }
}