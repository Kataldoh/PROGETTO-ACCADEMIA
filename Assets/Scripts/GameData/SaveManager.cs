using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    /*//Istanzia questo codice 
    public static SaveManager instance { get; private set; }

    //variabili da salvare
    public float[] playerposition; //nota: i Vector3 non si possono trasformare in codice binario perciò è necessario introdurre un array di float

    private void Awake()
    {
        // Distruggi eventuali copie del gamepbject che verranno caricate
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
         
    }

    //Metodo per caricare la partita
    public void LoadGame()
    {
        //controlla se li percorso file esiste
        if (File.Exists(Application.persistentDataPath + "/infogiocatore"))
        {
            //Ricarica le informazioni dal file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream savefile = File.Open(Application.persistentDataPath + "/infogiocatore", FileMode.Open);

            //converte i data in PlayerData_Storage in codice leggibile da Unity
            PlayerData_Storage data = (PlayerData_Storage)bf.Deserialize(savefile);
            //info da caricare
            playerposition = data.playerposition;

            //...altre info da caricare da inserire

            savefile.Close();

        }
    }

    
    //Metodo per salvare la partita
    public void SaveGame()
    {
        //Crea un nuovo file dove le inforamzioni verranno messe quando si salva il gioco
        BinaryFormatter bf = new BinaryFormatter();
        FileStream savefile = File.Create(Application.persistentDataPath + "/infogiocatore");
        
        //Crea i dati del giocatore nella classe PlayerData_Storage
        PlayerData_Storage data = new PlayerData_Storage();

        //Serializza il dato e chiude il file
        data.playerposition = playerposition;

        //...altre info da salvare da inserire

        bf.Serialize(savefile, data);
        savefile.Close();
    }*/

}

//In questa classe verranno inserite tutte le variabili che verranno trattentute prima che diventino codice binario
[Serializable]
class PlayerData_Storage
{
    public float[] playerposition;
}
