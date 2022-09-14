using System;
using TMPro;
using UnityEngine;

public class TextPopUpComponentObject : PopUpComponentObject
{
    [SerializeField] private TMP_Text TextObject;

    public override void SetData(PopUpComponentData unTypedData, Action closeOnUse)
    {
        TextPopUpComponentData data = unTypedData as TextPopUpComponentData;

        TextObject.text = data.TextContent;
    }
}
