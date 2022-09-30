using System;
using TMPro;
using UnityEngine;

public class PricePopUpComponentObject : MonoBehaviour, IPopUpComponentObject
{
    public PopUpComponentType ModuleConcept;

    [SerializeField] private TMP_Text PriceTextObject;

    public void SetData(IPopUpComponentData unTypedData, Action closeOnUse)
    {
        PricePopUpComponentData data = unTypedData as PricePopUpComponentData;

        PriceTextObject.text = data.PriceTextContent;
    }
}
