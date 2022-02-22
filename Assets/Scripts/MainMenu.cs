using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public void StartPlay()
    {
        //carica scensa successiva in elenco (gioco)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        //termina l'applicazione
        Application.Quit();
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene("MainMenu");
        //savedata.player_health = gamecontroller.CurrentHealth; 
    }

}
