using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExternalBoosterElementView : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new();

    private Action<ExternalBoosterSourceController> _onElementClicked;

    [SerializeField] private Image _externalBoosterImage;
    [SerializeField] private TMP_Text _externalBoosterAmount;

    private InventoryManager _inventory;

    public ExternalBoosterSourceController SpecificBoosterLogic;


    public void Initialize(ExternalBoosterSourceController boosterElementLogic, InventoryManager inventory, Action<ExternalBoosterSourceController> elementClickedEvent)
    {
        SpecificBoosterLogic = boosterElementLogic;

        gameObject.name = boosterElementLogic.boosterName;
        _onElementClicked = elementClickedEvent;
        _inventory = inventory;

        UpdateVisuals();
    }

    public void ExecuteBooster() => _onElementClicked?.Invoke(SpecificBoosterLogic);
    public void UpdateBoosterAmountText() => _externalBoosterAmount.text = _inventory.CheckElementAmount(SpecificBoosterLogic.boosterName).ToString();
    void UpdateVisuals()
    {
        _externalBoosterImage.sprite = _sprites.Find(sprite => sprite.name == SpecificBoosterLogic.boosterName);
        UpdateBoosterAmountText();
    }
}
