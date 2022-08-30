using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExternalBoosterElementView : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();

    private Action<ExternalBoosterBase> _onElementClicked;

    [SerializeField] private Image _externalBoosterImage;
    [SerializeField] private TMP_Text _externalBoosterAmount;

    private InventoryManager _inventory;

    public ExternalBoosterElementController Controller;


    public void Initialize(ExternalBoosterBase boosterElementModel, InventoryManager inventory, Action<ExternalBoosterBase> elementClickedEvent)
    {
        gameObject.name = boosterElementModel.boosterName;
        _onElementClicked = elementClickedEvent;
        Controller = new(boosterElementModel);
        _inventory = inventory;

        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (Controller.ExternalBoosterBehaviour == null)
            return;

        _externalBoosterImage.sprite = _sprites.Find(sprite => sprite.name == Controller.ExternalBoosterBehaviour.boosterName);

        SetBoosterAmount();
    }
    public void ExecuteBooster()
    {
        if (Controller.ExternalBoosterBehaviour == null)
            return;

        _onElementClicked?.Invoke(Controller.ExternalBoosterBehaviour);
    }

    public void ExternalBoosterUsedEffect()
    {
        SetBoosterAmount();
    }

    private void SetBoosterAmount()
    {
        _externalBoosterAmount.text = _inventory.CheckElementAmount(Controller.ExternalBoosterBehaviour.boosterName).ToString();
    }
}
