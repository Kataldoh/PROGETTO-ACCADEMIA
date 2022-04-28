using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public MainPlayerScript mainPlayerScript;
    public static GameController instance;
    public GameObject puntatore;
    public GameObject post_processing;
    public GameObject player;
    public GameObject BarraBoss;

    public float[] medikitEnergy;

    
    public Vector3 cameraOffset;
    public bool followPlayerX;
    public bool followPlayerY;
    public float cameraSmoothing;
    public Vector3 triggerPos;

    public int maxHealth = 100;
    public int CurrentHealth;
    public float CurrentStamina;
    public float[] playerposition;

    public int maxHealthBoss = 100;
    public int CurrentHealthBoss;


    public float maxStamina = 100;
    public float staminaRegen = 5f;
    public float staminaDrain = 2f;


    public BarraVita BarraVita;
    public BarraStamina BarraStamina;
    public BarraBoss barraBoss;

    public GameState _state;
    public bool eventpowerup;
    public GameObject[] pannelli;
    public GameObject[] testievento;

    StatesEvents _estates;

    private void Awake()
    {
        SoundManager.Initialize();
        BarraBoss = GameObject.FindGameObjectWithTag("Boss UI");
        player = GameObject.FindGameObjectWithTag("Player");
        //BarraBoss = GameObject.FindGameObjectWithTag("Boss UI");
        post_processing = GameObject.FindGameObjectWithTag("P.Process");
        puntatore = GameObject.FindGameObjectWithTag("Puntatore");
        instance = this;


        //**************** SAVE DATA TO PLAYER PREFS;
        //PlayerPrefs.SetInt("quality", 3);
        //DontDestroyOnLoad(this.gameObject);

    }


    // Start is called before the first frame update
    void Start()
    {
        _state = GameState.play;
        _estates = new StatesEvents();
        CurrentHealth = maxHealth;
        CurrentStamina = maxStamina;
        CurrentHealthBoss = maxHealthBoss;

       


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

        if (puntatore = null)
        {
            puntatore = GameObject.FindGameObjectWithTag("Puntatore");

        }

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

        if (CurrentHealth > maxHealth)
            CurrentHealth = maxHealth;

      
    }

    public void EnableBossBar(bool a)
    {
        BarraBoss.SetActive(a);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

            case GameState.eventpausa:
                _estates._EVENTPAUSE();
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

    public void SetMaxBossHealth(int hp)
    {
        barraBoss.SetMaxHealthBoss(hp);
    }
    public void SetBossHealth(float health)
    {
        barraBoss.SetHealthBoss(health);
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

    /*public void SavePlayer()
    {
        SaveSystem.SaveDataPlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerDataforSave data = SaveSystem.LoadPlayer();

        Player.position.x = playerposition.x;
        Vector3 playerposition;
        playerposition.x = data.playerposition[0];
        playerposition.y = data.playerposition[1];
        playerposition.z = data.playerposition[2];
        transform.position = playerposition;
    }*/


}

