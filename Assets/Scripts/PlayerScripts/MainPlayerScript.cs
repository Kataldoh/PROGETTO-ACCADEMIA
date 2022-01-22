using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayerScript : MonoBehaviour
{
    public static MainPlayerScript pInstance;
    public int maxHealth = 100;
    public int CurrentHealth;
    public Barra BarraVita;

    public PlayerData pdata; // SCRIPTABLE OBJECT che determina forza del salto,velocit� della rotazione,lunghezza del raycast frontale

    public PlayerState _state; // Stati del player
    [SerializeField] PlayerStatesEvents _Estates;

    [SerializeField] public Vector3 move;
    [SerializeField] float force;
    public CharacterController controller;
    [SerializeField] public bool isGrounded; //bool che determina se il player � a terra oppure no
    public bool isJump;
    [SerializeField] Transform foot;
    [SerializeField] Transform rayhead;
    [SerializeField] public LayerMask layer;
    [SerializeField] public Animator anim;

    //*****************************************
    [SerializeField] float JumpForce;
    [SerializeField] public float gravity;
    [SerializeField] public float weight;
    [SerializeField] float rayLenght;
    [SerializeField] public Transform cursor;
    [SerializeField] public float velocity;
    float rot=90;
    int dir;
    [SerializeField] float speedRot;
    private void Awake()
    {
        pInstance = this;
    }
    void Start()
    {
        _Estates = new PlayerStatesEvents();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        CurrentHealth = maxHealth;
    }

    private void Update()
    {

        if (CurrentHealth == 0)
        {
            GameController.instance._state = GameState.dead;
        }

        //Questo if serve solo per testare se il metodo TakeDamage funziona (Aldo)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }


    void FixedUpdate()
    {
      
        if (GameController.instance._state == GameState.play)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                GameController.instance._state = GameState.pause;
            }

            StateIndipendentActions();
            AnimationHandler();
            States();
            
        }
    }

    
    public bool IsGrounded()
    {
        return Physics.CheckSphere(foot.position, rayLenght, layer);
    }

    void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        BarraVita.SetHealth(CurrentHealth);
    }

    public void States() {
        switch (_state)
        {
            case PlayerState.idle:
                _Estates.P_Idle();
                break;
            case PlayerState.groundMoving:
                _Estates.P_Move();
                break;
            case PlayerState.jump:
                _Estates.P_Jump();
                break;
            case PlayerState.damage:
                _Estates.P_Damage();
                break;
            case PlayerState.dead:
                _Estates.P_Death();
                break;
        }
    }

    public void StateIndipendentActions()
    {
        move = new Vector3(
                   Input.GetAxis("Horizontal") * Time.deltaTime * 64,
                   Input.GetAxis("Vertical"),
                   0
                );
            

        isGrounded = IsGrounded();
        
        //determino la direzione nel quale il player guarda
        if (move.x > 0)
        {
            rot = 90;
            dir = 1;

        }
        else if (move.x < 0)
        {
            rot = 270;
            dir = -1;
        }

        //rotazione del player
        Quaternion qrot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rot, 0), Time.deltaTime * pdata.speedRot);
        transform.rotation = qrot;

        //Applico Gravità
        if (IsGrounded() && velocity <= 0)
        {
            velocity = weight;
            isJump = false;
        }
        else
        {
            velocity += gravity * Time.deltaTime;
        }
    }

    void AnimationHandler()
    {
        switch (_state)
        {
            case PlayerState.idle:
                anim.SetBool("jump", false);
                anim.SetFloat("posx", 0, 0.05f, Time.deltaTime);
                anim.SetFloat("posy", 0, 0.15f, Time.deltaTime);
                break;
            case PlayerState.groundMoving:
                anim.SetBool("jump", false);
                anim.SetFloat("posx", move.x, 0.05f, Time.deltaTime);
                anim.SetFloat("posy", move.y, 0.15f, Time.deltaTime);
                break;
            case PlayerState.jump:
                anim.SetBool("jump", true);
                break;
            case PlayerState.damage:
                _Estates.P_Damage();
                break;
            case PlayerState.dead:
                _Estates.P_Death();
                break;
        }
    }

    /*
    public bool IsInRayCastDireciton(Vector3 direction, float lenght, LayerMask layer) {
        Debug.DrawRay(rayhead.position, direction * lenght, Color.red);
        return Physics.Raycast(rayhead.position, direction, out RaycastHit hit, lenght, layer);
    }
    */

}
