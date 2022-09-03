using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarshipColorsView : MonoBehaviour
{
    public TMP_Text PackHeaderText;
    public Image[] PackColorsDisplay;

    private DeSeializedStarshipColors _colorsSkin;

    private Action<DeSeializedStarshipColors> interactEvent;
    public void InitStarshipColorView(DeSeializedStarshipColors skin, Action<DeSeializedStarshipColors> onInteract)
    {
        _colorsSkin = skin;
        interactEvent = onInteract;

        PackHeaderText.text = _colorsSkin.SkinName;
        
        for (int i = 0; i < _colorsSkin.SkinColors.Length; i++)
            PackColorsDisplay[i].color = _colorsSkin.SkinColors[i];
    }

    public void InteractWithColor() => interactEvent?.Invoke(_colorsSkin);
}
