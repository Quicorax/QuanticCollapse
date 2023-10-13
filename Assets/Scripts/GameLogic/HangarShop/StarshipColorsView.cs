using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace QuanticCollapse
{
    public class StarshipColorsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _packHeaderText;
        [SerializeField] private Image[] _packColorsDisplay;

        [SerializeField] private GameObject _lockImage;

        private DeSeializedStarshipColors _colorsSkin;

        private Action<DeSeializedStarshipColors, Action> _interactEvent;

        public void InitStarshipColorView(DeSeializedStarshipColors skin, bool isLocked,
            Action<DeSeializedStarshipColors, Action> onInteract)
        {
            _colorsSkin = skin;
            _interactEvent = onInteract;
            _lockImage.SetActive(isLocked);
            _packHeaderText.text = _colorsSkin.SkinName;

            for (int i = 0; i < _colorsSkin.SkinColors.Length; i++)
            {
                _packColorsDisplay[i].color = _colorsSkin.SkinColors[i];
            }
        }

        public void InteractWithColor() => _interactEvent?.Invoke(_colorsSkin, PurchaseConfirmation);

        void PurchaseConfirmation() => _lockImage.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => _lockImage.SetActive(false));
    }
}