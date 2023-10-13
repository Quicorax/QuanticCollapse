using TMPro;
using UnityEngine;

namespace QuanticCollapse
{
    public class LocalizeText : MonoBehaviour
    {
        private void Awake()
        {
            if (TryGetComponent(out TMP_Text text))
            {
                var localization = ServiceLocator.GetService<LocalizationService>();
                text.text = localization.Localize(text.text);
            }
        }
    }
}