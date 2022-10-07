using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


public static class GameDataUpdater
{
    [Serializable]
    private class Language
    {
        public List<LanguajeDictionary.LocalizationEntry> data;
    }

    [Serializable]
    private class Languages
    {
        public Language English;
        public Language Spanish;
        public Language Catalan;
    }

    [MenuItem("Game/Update Localization Data")]
    public static void UpdateLocalizationData() => UpdateLocalization("https://script.google.com/macros/s/AKfycbx0Rk2fwAcyOyuX1LMx9bnGiwRkEYkrJraZh13fa5TumppAQABW7l_Z8x6zNS5q5TDv/exec");

    public static UnityWebRequest WebRequest(string url) { return new UnityWebRequest(url, "GET", new DownloadHandlerBuffer(), null); }
   
    public static void UpdateLocalization(string url)
    {
        UnityWebRequest request = WebRequest(url);
        request.SendWebRequest().completed += operation =>
        {
            if (request.error != null)
            {
                Debug.Log(request.error);
                return;
            }

            Debug.Log("Localization Data updated with -> " + request.downloadHandler.text);

            Languages languages = JsonUtility.FromJson<Languages>(request.downloadHandler.text);
            var english = languages.English;
            var spanish = languages.Spanish;
            var catalan = languages.Catalan;

            System.IO.File.WriteAllText(Application.dataPath + "/InitialResources/English_file.json",
                JsonUtility.ToJson(english));
            System.IO.File.WriteAllText(Application.dataPath + "/InitialResources/Spanish_file.json",
                JsonUtility.ToJson(spanish));
            System.IO.File.WriteAllText(Application.dataPath + "/InitialResources/Catalan_file.json",
                JsonUtility.ToJson(catalan));
            AssetDatabase.Refresh();
        };
    }
}