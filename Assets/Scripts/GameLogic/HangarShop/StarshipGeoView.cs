using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QuanticCollapse
{
    public class StarshipGeoView : MonoBehaviour
    {
        [SerializeField] private List<Sprite> sprites = new();
        [SerializeField] private Image image;
        [SerializeField] private GameObject lockImage;
        private StarshipGeoModel _geo;

        private Action<StarshipGeoModel, Action> _interactEvent;
        public void InitStarshipGeoView(StarshipGeoModel geo, bool isLocked, Action<StarshipGeoModel, Action> onInteract)
        {
            _geo = geo;

            lockImage.SetActive(isLocked);
            _interactEvent = onInteract;

            image.sprite = sprites.Find(x => x.name == geo.StarshipName);
        }

        public void InteractWithGeo() => _interactEvent?.Invoke(_geo, PurchaseConfirmation);

        void PurchaseConfirmation() => lockImage.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            lockImage.SetActive(false);
        });
    }
}