using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    //Istanzia questo codice 
    public static SaveManager instance { get; private set; }
    //variabili da salvare
    //public float[] playerposition; //nota: i Vector3 non si possono trasformare in codice binario perciò è necessario introdurre un array di float

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
    public void SaveGame()
    {
        //Salva le coordinate del Player
        PlayerPrefs.SetFloat("Coordinatax", SaveSystem.instance.playerposition[0]);
        PlayerPrefs.SetFloat("Coordinatay", SaveSystem.instance.playerposition[1]);
        PlayerPrefs.SetFloat("Coordinataz", SaveSystem.instance.playerposition[2]);
        PlayerPrefs.SetInt("HealthPlayer", SaveSystem.instance.healthplayer);
    }
    
    //Metodo per caricare la partita
    public void LoadGame()
    {
        try
        {
            SaveSystem.instance.playerposition[0] = PlayerPrefs.GetFloat("Coordinatax");
            SaveSystem.instance.playerposition[1] = PlayerPrefs.GetFloat("Coordinatay");
            SaveSystem.instance.playerposition[2] = PlayerPrefs.GetFloat("Coordinataz");
            SaveSystem.instance.healthplayer = PlayerPrefs.GetInt("HealthPlayer");
        }
        catch
        {
            return;

        }
    }
}