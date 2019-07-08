using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
    public static void save(int lives, string sceneName, bool loadNotAllowed)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/run.tank";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        SaveLoadObject data = new SaveLoadObject(lives, sceneName, loadNotAllowed);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveLoadObject load()
    {
        string path = Application.persistentDataPath + "/run.tank";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveLoadObject data = formatter.Deserialize(stream) as SaveLoadObject;
            stream.Close();

            if (!data.isOpened())
            {
                save(0, "", true);
                return data;
            }

            return null;
        } else
        {
            return null;
        }
    }
}
