using UnityEngine;
using System.IO;

public static class SaveManager
{

    public static string directory = "/SaveData/";
    public static string fileName = "savedata.txt";

    public static void Save(PlayerInventoryData data)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(dir + fileName, json);
    }

    public static PlayerInventoryData Load()
    {
        string path = Application.persistentDataPath + directory + fileName;
        Debug.Log(path);
        PlayerInventoryData data = new PlayerInventoryData();

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            data = JsonUtility.FromJson<PlayerInventoryData>(jsonData);
        }
        else
        {
            Debug.Log("Missing save data");
        }

        return data;
    }
    
}
