using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public MainPlayerScript MainPlayerScript;
    public static GameController instance;
    public GameObject puntatore;
    public GameObject post_processing;

    public int maxHealth = 100;
    public int CurrentHealth;


    public float maxStamina = 100;
    public float CurrentStamina;
    public float staminaRegen = 5f;
    public float staminaDrain = 2f;


    public BarraVita BarraVita;
    public BarraStamina BarraStamina;

    public GameState _state;
    public GameObject[] pannelli;

    StatesEvents _estates;

    private void Awake()
    {
       
        puntatore = GameObject.FindGameObjectWithTag("Puntatore");
        instance = this;

        //**************** SAVE DATA TO PLAYER PREFS;
        //PlayerPrefs.SetInt("quality", 3);
        DontDestroyOnLoad(this.gameObject);

    }


    // Start is called before the first frame update
    void Start()
    {
        _state = GameState.play;
        _estates = new StatesEvents();
        CurrentHealth = maxHealth;
        CurrentStamina = maxStamina;


        //***************** LOAD DATA FROM PlayerPrefs;
        /*
        int q=PlayerPrefs.GetInt("quality");

        if (q == 3) {
            post_processing.SetActive(false);
        }
        else
        {
            post_processing.SetActive(true);
        }

        QualitySettings.SetQualityLevel(q);
        */

    }

    // Update is called once per frame
    void Update()
    {
       
        States();

        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameController.instance._state == GameState.pause)
            {
                GameController.instance._state = GameState.play;
                Debug.Log("RESUMING");

            }
            else
            {
                GameController.instance._state = GameState.pause;

            }

        }



      
    }
    public void Restart()
    {
        SceneManager.LoadScene("game");
    }
    public void Resume()
    {
        Debug.Log("RESUMING");

        GameController.instance._state = GameState.play;

    }

    public void Quit()
    {
        Debug.Log("QUITTING");
        Application.Quit();
    }


    public void States() {
        switch (_state)
        {

            case GameState.idle:
                _estates._IDLE();
                break;

            case GameState.play:
                _estates._PLAY();
                break;

            case GameState.dead:
                _estates._DEAD();
                break;

            case GameState.pause:
                _estates._PAUSE();
                break;


        }
    }

    //---------------
    //Metodi Generici
    //---------------

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        BarraVita.SetHealth(CurrentHealth);
    }

    public void TakeStamina()
    {
        //Debug.Log("La stamina scende");
        
            CurrentStamina -= staminaDrain * Time.deltaTime;
            BarraStamina.SetStamina(CurrentStamina);

        if (CurrentStamina < 0)
        {
            CurrentStamina = 0;
        }
        
    }

    public void RegenStamina()
    {

        //Debug.Log("La stamina si rigenera");

        if (CurrentStamina <= maxStamina - 0.01)
        {
            CurrentStamina += staminaRegen * Time.deltaTime;
            BarraStamina.SetStamina(CurrentStamina);
        }

      
    }

}

