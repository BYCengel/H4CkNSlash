using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveLevel(LevelGeneration levels)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/level.lopcuk";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(levels);
        
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("SAVED");
    }

    public static LevelData LoadLevel()
    {
        string path = Application.persistentDataPath + "/level.lopcuk";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();
            return data;
        }
        else
        {
            //Debug.LogError("Kayıt Datası bulunamadı where -> " + path);
            return null;
        }
    }
}
