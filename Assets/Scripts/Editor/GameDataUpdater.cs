using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public static class GameDataUpdater
{
    [MenuItem("Game/Update All")]
    public static void UpdateAll()
    {
        UpdateShopModel();
        UpdateLevelModel();
        UpdateStarshipColorsModel();
        UpdateStarshipGeoModel();
    }

    [MenuItem("Game/Update Shop Data")]
    public static void UpdateShopModel() => UpdateRemoteResource("ShopElements", "https://script.google.com/macros/s/AKfycbyqWYKBcB31cnCl7YrjmJn6jlXZCPxiJTFIXZg9sM99ec322SdqhuuyVOQqqAW8iSyB4A/exec");

    [MenuItem("Game/Update Levels Data")]
    public static void UpdateLevelModel() => UpdateRemoteResource("Levels", "https://script.google.com/macros/s/AKfycbwTDEcnhXzTA9jgERQsMcI7QdR7JT9PGmuQMhiPao3jLgmcVFIbJgZMh-PRBglhsGAM/exec");

    [MenuItem("Game/Update StarshipColors Data")]
    public static void UpdateStarshipColorsModel() => UpdateRemoteResource("StarshipColors", "https://script.google.com/macros/s/AKfycbzmoOarDMhtm5UWnxVqist_EEzwk9VsJf_9YP4Jup1F9WmHN8IcSilnsYSem9-aRozFvg/exec");
    [MenuItem("Game/Update StarshipGeo Data")]
    public static void UpdateStarshipGeoModel() => UpdateRemoteResource("StarshipGeo", "https://script.google.com/macros/s/AKfycby-8doTKx5f5le1EMiB4RwV6Tc16bFQJhS_047ppqNBEXTkYjSK4ieGCUVHHHlLxqTE/exec");

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

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/" + resource + ".json", request.downloadHandler.text);
            Debug.Log(resource + " updated with -> " + request.downloadHandler.text);

            AssetDatabase.Refresh();
        };
    }
}