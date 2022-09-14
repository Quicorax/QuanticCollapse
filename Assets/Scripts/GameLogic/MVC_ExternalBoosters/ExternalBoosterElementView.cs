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

    public ExternalBoosterSourceController SpecificBoosterLogic;
    private GameProgressionService _gameProgression;


    public void Initialize(ExternalBoosterSourceController boosterElementLogic, GameProgressionService gameProgression, Action<ExternalBoosterSourceController> elementClickedEvent)
    {
        SpecificBoosterLogic = boosterElementLogic;

        gameObject.name = boosterElementLogic.boosterName;
        _onElementClicked = elementClickedEvent;
        _gameProgression = gameProgression;

        UpdateVisuals();
    }

    public void ExecuteBooster() => _onElementClicked?.Invoke(SpecificBoosterLogic);
    public void UpdateBoosterAmountText() => _externalBoosterAmount.text = _gameProgression.CheckElement(SpecificBoosterLogic.boosterName).ToString();
    void UpdateVisuals()
    {
        _externalBoosterImage.sprite = _sprites.Find(sprite => sprite.name == SpecificBoosterLogic.boosterName);
        UpdateBoosterAmountText();
    }
}
