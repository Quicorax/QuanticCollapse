using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public static class GameDataUpdater
{
    const string shopURL = "https://script.google.com/macros/s/AKfycbyqWYKBcB31cnCl7YrjmJn6jlXZCPxiJTFIXZg9sM99ec322SdqhuuyVOQqqAW8iSyB4A/exec";

    [MenuItem("Game/Update Shop Data")]
    public static void UpdateShopModel()
    {
        UnityWebRequest request = new UnityWebRequest(shopURL, "GET", new DownloadHandlerBuffer(), null);
        request.SendWebRequest().completed += asyncOp =>
        {
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                return;
            }

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/ShopElements.json", request.downloadHandler.text);
            Debug.Log("ShopElements updated with -> " + request.downloadHandler.text);
        };
    }
}