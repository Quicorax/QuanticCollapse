using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public static class GameDataUpdater
{
    const string shopURL = "https://script.google.com/macros/s/AKfycbyqWYKBcB31cnCl7YrjmJn6jlXZCPxiJTFIXZg9sM99ec322SdqhuuyVOQqqAW8iSyB4A/exec";
    const string levelsURL = "https://script.google.com/macros/s/AKfycbwTDEcnhXzTA9jgERQsMcI7QdR7JT9PGmuQMhiPao3jLgmcVFIbJgZMh-PRBglhsGAM/exec";
    const string colorsURL = "https://script.google.com/macros/s/AKfycbzmoOarDMhtm5UWnxVqist_EEzwk9VsJf_9YP4Jup1F9WmHN8IcSilnsYSem9-aRozFvg/exec";

    const string Resources = "/Resources/";
    const string JSON = ".json";


    [MenuItem("Game/Update Shop Data")]
    public static void UpdateShopModel() => UpdateRemoteResource("ShopElements", shopURL);

    [MenuItem("Game/Update Levels Data")]
    public static void UpdateLevelModel() => UpdateRemoteResource("Levels", levelsURL);

    [MenuItem("Game/Update Colors Data")]
    public static void UpdateColorModel() => UpdateRemoteResource("Colors", colorsURL);

    public static UnityWebRequest WebRequest(string url) { return new UnityWebRequest(url, "GET", new DownloadHandlerBuffer(), null); }
    private static void UpdateRemoteResource(string resource, string url)
    {
        UnityWebRequest request = WebRequest(url);
        request.SendWebRequest().completed += asyncOp =>
        {
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                return;
            }

            System.IO.File.WriteAllText(Application.dataPath + Resources + resource + JSON, request.downloadHandler.text);
            Debug.Log(resource + " updated with -> " + request.downloadHandler.text);

            AssetDatabase.Refresh();
        };
    }
}