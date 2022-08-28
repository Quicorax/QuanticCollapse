using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public static class GameDataUpdater
{
    const string shopURL = "https://script.google.com/macros/s/AKfycbyqWYKBcB31cnCl7YrjmJn6jlXZCPxiJTFIXZg9sM99ec322SdqhuuyVOQqqAW8iSyB4A/exec";
    const string levelsURL = "https://script.google.com/macros/s/AKfycbwTDEcnhXzTA9jgERQsMcI7QdR7JT9PGmuQMhiPao3jLgmcVFIbJgZMh-PRBglhsGAM/exec";

    public static UnityWebRequest WebRequest(string url) { return new UnityWebRequest(url, "GET", new DownloadHandlerBuffer(), null); }

    [MenuItem("Game/Update Shop Data")]
    public static void UpdateShopModel()
    {
        UnityWebRequest request = WebRequest(shopURL);
        request.SendWebRequest().completed += asyncOp =>
        {
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                return;
            }

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/ShopElements.json", request.downloadHandler.text);
            Debug.Log("ShopElements updated with -> " + request.downloadHandler.text);

            AssetDatabase.Refresh();
        };
    }
    [MenuItem("Game/Update Levels Data")]
    public static void UpdateLevelModel()
    {
        UnityWebRequest request = WebRequest(levelsURL);
        request.SendWebRequest().completed += asyncOp =>
        {
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                return;
            }

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/Levels.json", request.downloadHandler.text);
            Debug.Log("Levels updated with -> " + request.downloadHandler.text);

            AssetDatabase.Refresh();
        };
    }
}