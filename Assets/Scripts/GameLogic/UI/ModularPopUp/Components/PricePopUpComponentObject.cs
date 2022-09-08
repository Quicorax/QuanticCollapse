using System;
using TMPro;
using UnityEngine;

public class PricePopUpComponentObject : PopUpComponentObject
{
    [SerializeField] private TMP_Text PriceTextObject;

    public override void SetData(PopUpComponentData unTypedData, Action closeOnUse)
    {
        PricePopUpComponentData data = unTypedData as PricePopUpComponentData;

        PriceTextObject.text = data.PriceTextContent;
    }
}
