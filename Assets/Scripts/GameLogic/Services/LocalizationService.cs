using UnityEngine;


namespace QuanticCollapse
{
    public class LocalizationService : IService
    {
        private LanguajeDictionary _currentLanguaje;
        public void Initialize(string defaultLanguajeFile, bool forceLanguaje = false)
        {
            if (forceLanguaje)
            {
                _currentLanguaje = Resources.Load<LanguajeDictionary>(defaultLanguajeFile);
            }
            else
            {
                _currentLanguaje = Resources.Load<LanguajeDictionary>(Application.systemLanguage.ToString()) ??
                               Resources.Load<LanguajeDictionary>(defaultLanguajeFile);
            }

            _currentLanguaje?.Initialize();
        }
        public string Localize(string key) => _currentLanguaje?.Localize(key) ?? key;

        public void Clear() { }
    }
}