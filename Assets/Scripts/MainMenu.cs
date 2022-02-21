using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public void QualityLevelLow()
    {
        QualitySettings.SetQualityLevel(0, true);
    }
    public void QualityLevelMed()
    {
        QualitySettings.SetQualityLevel(1, true);
    }
    public void QualityLevelHigh()
    {
        QualitySettings.SetQualityLevel(2, true);
    }
}
