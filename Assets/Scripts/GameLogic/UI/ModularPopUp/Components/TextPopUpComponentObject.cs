using System;
using TMPro;
using UnityEngine;

public class TextPopUpComponentObject : MonoBehaviour, IPopUpComponentObject
{
    public PopUpComponentType ModuleConcept;

    [SerializeField] private TMP_Text TextObject;

    public void SetData(IPopUpComponentData unTypedData, Action closeOnUse)
    {
        TextPopUpComponentData data = unTypedData as TextPopUpComponentData;

        TextObject.text = data.TextContent;
    }
}
