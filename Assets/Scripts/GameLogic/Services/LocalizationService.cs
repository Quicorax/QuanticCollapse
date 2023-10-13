using UnityEngine;


namespace QuanticCollapse
{
    public class LocalizationService : IService
    {
        private LanguageDictionary _currentLanguage;

        public void Initialize(string defaultLanguageFile, bool forceLanguage = false)
        {
            if (forceLanguage)
            {
                _currentLanguage = Resources.Load<LanguageDictionary>(defaultLanguageFile);
            }
            else
            {
                _currentLanguage = Resources.Load<LanguageDictionary>(Application.systemLanguage.ToString())
                                   ?? Resources.Load<LanguageDictionary>(defaultLanguageFile);
            }

            _currentLanguage?.Initialize();
        }

        public string Localize(string key) => _currentLanguage?.Localize(key) ?? key;

        public void Clear()
        {
        }
    }
}