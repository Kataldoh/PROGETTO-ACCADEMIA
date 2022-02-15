using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveDataPlayer(MainPlayerScript player)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string location = Application.persistentDataPath + "/infogiocatore";
        FileStream stream = new FileStream(location, FileMode.Create);

        PlayerDataforSave data = new PlayerDataforSave(player);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerDataforSave LoadPlayer()
    {
        string path = Application.persistentDataPath + "/infogiocatore";
        if(File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerDataforSave data =bf.Deserialize(stream) as PlayerDataforSave;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
