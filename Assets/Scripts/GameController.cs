using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject puntatore;

    public int maxHealth = 100;
    public int CurrentHealth;
    public Barra BarraVita;
    public GameState _state;
    public GameObject[] pannelli;

    StatesEvents _estates;

    private void Awake()
    {
       
        puntatore = GameObject.FindGameObjectWithTag("Puntatore");
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _state = GameState.play;
        _estates = new StatesEvents();
        CurrentHealth = maxHealth;
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

}

