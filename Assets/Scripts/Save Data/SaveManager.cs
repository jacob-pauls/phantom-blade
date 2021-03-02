using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public static class SaveManager 
{
    public static void Save(GameSaveFile data, string fileName)
    {
        // Path to the file
        string path = Application.persistentDataPath + "/" + fileName + ".data";
        BinaryFormatter bf = new BinaryFormatter();
        using (var stream = File.Create(path))
        {
            bf.Serialize(stream, data);
        }
        Debug.Log("Saved " + data + " to " + path);
    }

    public static GameSaveFile Load(string fileName)
    {
        // Local variable set to null
        GameSaveFile data = null;
        string path = Application.persistentDataPath + "/" + fileName + ".data";
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(path))
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                data = (GameSaveFile)bf.Deserialize(stream);
            }

            //Debug.Log("File loaded " + fileName);

        }
        else // Does not exist
        {
            Debug.Log("File " + fileName + " does not exist");
        }
        return data;
    }
}
