using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImagePopUpComponentObject : MonoBehaviour, IPopUpComponentObject
{
    public PopUpComponentType ModuleConcept;

    [SerializeField] private List<Sprite> sprites = new();

    [SerializeField] private Image ImageDisplay;
    [SerializeField] private GameObject ImageTextGameObject;
    [SerializeField] private TMP_Text ImageTextObject;

    public void SetData(IPopUpComponentData unTypedData, Action closeOnUse)
    {
        ImagePopUpComponentData data = unTypedData as ImagePopUpComponentData;

        ImageDisplay.sprite = sprites.Find(img => img.name == data.SpriteName);
   
       if (data.WithText) 
       { 
           ImageTextGameObject.SetActive(true);
           ImageTextObject.text = data.ImageText;
       }
    }
}