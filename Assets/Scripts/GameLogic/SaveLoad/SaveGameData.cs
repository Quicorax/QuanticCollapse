using UnityEngine;
using System.IO;

public class SaveGameData
{
    const string _slash = "/";
    const string _radical = ".txt";

    public void Save(SerializableSaveData saveFile)
    {
        string dataJson = JsonUtility.ToJson(saveFile);

        File.WriteAllText(Application.persistentDataPath + _slash + Application.productName + _radical, dataJson);
    }

    public SerializableSaveData Load()
    {
        string filePath = Application.persistentDataPath + _slash + Application.productName + _radical;

        if (!File.Exists(filePath))
            return new();

        string dataJson = File.ReadAllText(filePath);

        SerializableSaveData saveFile = JsonUtility.FromJson<SerializableSaveData>(dataJson);

        return saveFile;
    }
}
