using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("Main Menu");
        //savedata.player_health = gamecontroller.CurrentHealth; 
    }
}
