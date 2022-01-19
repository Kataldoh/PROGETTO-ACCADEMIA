using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameState _state;
    public GameObject[] pannelli;

    StatesEvents _estates;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _state = GameState.play;
        _estates = new StatesEvents();
    }

    // Update is called once per frame
    void Update()
    {
        States();
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

}

