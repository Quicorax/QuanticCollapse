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

    private MasterSceneManager _masterSceneManager;

    public ExternalBoosterElementController Controller;


    public void Initialize(ExternalBoosterBase boosterElementModel, MasterSceneManager master, Action<ExternalBoosterBase> elementClickedEvent)
    {
        gameObject.name = boosterElementModel.boosterName;
        _onElementClicked = elementClickedEvent;
        Controller = new(boosterElementModel);
        _masterSceneManager = master;

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
        _externalBoosterAmount.text = _masterSceneManager.Inventory.CheckElementAmount(Controller.ExternalBoosterBehaviour.boosterName).ToString();
    }
}
