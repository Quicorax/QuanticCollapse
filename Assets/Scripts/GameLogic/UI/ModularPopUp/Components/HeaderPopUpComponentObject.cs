using System;
using TMPro;
using UnityEngine;

namespace QuanticCollapse
{
    public class HeaderPopUpComponentObject : MonoBehaviour, IPopUpComponentObject
    {
        public PopUpComponentType ModuleConcept;

        [SerializeField] private GameObject BasicHeaderObject;
        [SerializeField] private TMP_Text BasicHeaderTextObject;
        [SerializeField] private GameObject HighlightedHeaderObject;
        [SerializeField] private TMP_Text HighlightedHeaderTextObject;

        public void SetData(IPopUpComponentData unTypedData, Action closeOnUse)
        {
            var data = unTypedData as HeaderPopUpComponentData;

            BasicHeaderObject.SetActive(!data.IsHeaderHighlighted);
            HighlightedHeaderObject.SetActive(data.IsHeaderHighlighted);

            if (data.IsHeaderHighlighted)
            {
                HighlightedHeaderTextObject.text = data.HeaderTextContent;
            }
            else
            {
                BasicHeaderTextObject.text = data.HeaderTextContent;
            }
        }
    }
}