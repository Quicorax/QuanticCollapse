using System;
using TMPro;
using UnityEngine;
public class HeaderPopUpComponentObject : PopUpComponentObject
{
    [SerializeField] private GameObject BasicHeaderObject;
    [SerializeField] private TMP_Text BasicHeaderTextObject;
    [SerializeField] private GameObject HighlightedHeaderObject;
    [SerializeField] private TMP_Text HighlightedHeaderTextObject;
    public override void SetData(PopUpComponentData unTypedData, Action closeOnUse)
    {
        HeaderPopUpComponentData data = unTypedData as HeaderPopUpComponentData;

        BasicHeaderObject.SetActive(!data.IsHeaderHighlighted);
        HighlightedHeaderObject.SetActive(data.IsHeaderHighlighted);

        if (data.IsHeaderHighlighted)
            HighlightedHeaderTextObject.text = data.HeaderTextContent;
        else
            BasicHeaderTextObject.text = data.HeaderTextContent;
    }
}
