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

        DontDestroyOnLoad(gameObject);
         
    }

    //Metodo per salvare la partita
    public void SaveGame(MainPlayerScript player)
    {
        playerposition[0] = player.pp.x;
        playerposition[1] = player.pp.y;
        playerposition[2] = player.pp.z;

        //Salva le coordinate del Player
        PlayerPrefs.SetFloat("Coordinatax", playerposition[0]);
        PlayerPrefs.SetFloat("Coordinatay", playerposition[1]);
        PlayerPrefs.SetFloat("Coordinataz", playerposition[2]);
    }
    
    //Metodo per caricare la partita
    public void LoadGame(MainPlayerScript player)
    {
        PlayerPrefs.GetFloat("Coordinatax");
        PlayerPrefs.GetFloat("Coordinatay");
        PlayerPrefs.GetFloat("Coordinataz");
    }
}