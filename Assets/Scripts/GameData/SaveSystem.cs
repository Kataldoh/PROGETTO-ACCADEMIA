using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
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
    private void Update()
    {
        //Sistema di salvataggio
        if (Input.GetKey(KeyCode.O))
        {
            playerposition[0] = MainPlayerScript.pInstance.transform.position.x;
            playerposition[1] = MainPlayerScript.pInstance.transform.position.y;
            playerposition[2] = MainPlayerScript.pInstance.transform.position.z;
            SaveManager.instance.SaveGame();
        }

        //SISTEMA DI CARICAMENTO
        if (Input.GetKeyDown(KeyCode.L))
        { 
            MainPlayerScript.pInstance.transform.position = new Vector3(playerposition[0], playerposition[1], playerposition[2]);
            SaveManager.instance.LoadGame();
           
        }
    }
}
    