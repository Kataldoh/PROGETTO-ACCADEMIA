using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    //Istanzia questo codice 
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
         
    }

    //Metodo per caricare la partita
    public void LoadGame()
    {

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
        bf.Serialize(savefile, data);
        savefile.Close();
    }

}

//In questa classe verranno inserite tutte le variabili che verranno trattentute prima che diventino codice binario
[Serializable]
class PlayerData_Storage
{
    public float[] playerposition;
}
