using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public float[] playerposition; //nota: i Vector3 non si possono trasformare in codice binario perciò è necessario introdurre un array di float
    public int healthplayer;
    public bool saving = false;
    public bool loading = false;
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



    public void SavePositions() {
        try
        {
            playerposition[0] = MainPlayerScript.pInstance.transform.position.x;
            playerposition[1] = MainPlayerScript.pInstance.transform.position.y;
            playerposition[2] = MainPlayerScript.pInstance.transform.position.z;
            healthplayer = GameController.instance.CurrentHealth;

            SaveManager.instance.SaveGame();
            print("Salvataggio effettuato");
        }
        catch {
            print("NON RIESCO A SALVARE!!!!");
            return;
                }
    }

    public void LoadPositions() {
        try
        {
            MainPlayerScript.pInstance.transform.position = new Vector3(playerposition[0], playerposition[1], playerposition[2]);
            SaveManager.instance.LoadGame();
            print("Caricamento corretto");
        }
        catch {
            print("Errore di caricamento...");
            return;
        }
    }

    private void Update()
    {
        //QUANDO SALVA
        if (/*Input.GetKey(KeyCode.O)*/saving)
        {

            saving = false;
        }

        //QUANDO CARICA
        if (/*Input.GetKeyDown(KeyCode.L)*/loading)
        { 
        
            loading = false;
        }
    }
}
    