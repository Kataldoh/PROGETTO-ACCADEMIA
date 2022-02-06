using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayerScript : MonoBehaviour
{
    public static MainPlayerScript pInstance;   //creo un'istanza del player
    public WeaponStats[] weapons_SO;

    [SerializeField] GameObject[] projectiles;

    public PlayerData pdata; // SCRIPTABLE OBJECT che determina forza del salto,velocit� della rotazione,lunghezza del raycast frontale

    public PlayerState _state; // Stati del player
    [SerializeField] PlayerStatesEvents _Estates;
    [SerializeField] public Vector3 move;
    [SerializeField] TrailRenderer dashTrail;
    public float speed;
    public CharacterController controller;
    [SerializeField] public bool isGrounded; //bool che determina se il player � a terra oppure no
    public bool isJump;
    public bool isDash;
    [SerializeField] Transform foot;    //posizione del "piede" del player, dove la sfera per trovare se si è a terra sarà situata
    [SerializeField] Transform rayhead;
    [SerializeField] public LayerMask layer;
    [SerializeField] public Animator anim;

    //*****************************************
    [SerializeField] float JumpForce;   //forza del salto
    [SerializeField] public float gravity;
    [SerializeField] public float weight;   //peso a terra del player
    [SerializeField] float radLenght;       //valore del raggio della sfera ai piedi del player per controllare se è a terra
    [SerializeField] public float velocity;
    float rot=90;
    public float dirX;
    public int dir;     //direzione nella quale il character si gira
    float startingZ;
    public LineRenderer laserRender;
    WeaponMethods aM;
    public float dashRechargeTime;
    public float dashTimer;
    public Vector3 lastShotPosition;
    public float height;
    public float hangTime;    //(da implementare) un hangtime per dare al giocatore una finestra per saltare dopo essere in aria
    
    private void Awake()
    {
        pInstance = this;   //Assegno l'istanza di questo player
    }
    void Start()
    {
        
        aM = new WeaponMethods();
        _Estates = new PlayerStatesEvents();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        laserRender = GetComponent<LineRenderer>();
        startingZ = transform.position.z;
        height = controller.height;
    }

    private void Update()
    {
        //Mantiene la posizione della Z costante a quella iniziale
        if(transform.position.z != startingZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startingZ);
        }

        //Se la vita del player è a 0
        if (GameController.instance.CurrentHealth == 0)
        {
            _state= PlayerState.dead;
            GameController.instance._state = GameState.dead;
        }

        if(dashTimer < dashRechargeTime)
            dashTimer += Time.deltaTime;
        else
            dashTimer = dashRechargeTime;

        if(Input.GetButtonDown("Fire3") && dashTimer >= dashRechargeTime)
            isDash = true;
            
        aM.ScreenAiming(rayhead);
        aM.GeneralWeaponHandler(weapons_SO[0], rayhead, projectiles);
    }


    void FixedUpdate()
    {
        //Il player non eseguirà alcuna azione se lo stato non è in play
        if (GameController.instance._state == GameState.play)
        {

            StateIndipendentActions();  //metodo per la gestione di azioni indipendenti dagli stati
            AnimationHandler(); //metodo per la gestione delle animazioni
            States();       //metodo per la gestione degli stati
        }
    }

    
    public bool IsGrounded()
    {
        if(!Physics.CheckSphere(foot.position, radLenght, layer))
        {
            hangTime += Time.deltaTime;
            if(hangTime >= 0.25f)
            {
                return false;
            }
            else
            {
                return true;
            }  
        }
        else
        {
            hangTime=0;
            return true;
        }
            
            
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
            case PlayerState.dash:
                _Estates.P_Dash();
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

        //Registra il movimento sugli assi
        move = new Vector3(
                   Input.GetAxis("Horizontal") * Time.deltaTime * speed,
                   Input.GetAxis("Vertical"),
                   0
                );
        
        //Mette a zero move.y per prevenire salti più alti ed errori d'animazione
        if(move.y >0)
        {
            move.y=0;
        }
            
        //Assegno il metodo per controllare se si è a terra ad una variabile
        isGrounded = IsGrounded();

        print(dirX);
        //determino la direzione nel quale il player guarda
        if (move.x > 0 || dirX > 0)
        {
            rot = 90;
            dir = 1;
        }
        else if (move.x < 0 || dirX < 0)
        {
            rot = 270;
            dir = -1;
        }

        //rotazione del player
        Quaternion qrot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rot, 0), Time.deltaTime * pdata.speedRot);
        transform.rotation = qrot;

        //Applico Gravità
        if (IsGrounded() && velocity <= 0)  //Se a terra con velocity <= 0
        {
            velocity = weight;        //Resetta la velocity dandogli un peso (tipicamente 0)
            isJump = false;         //mette a falso la variabile di salto per permettere di saltare sucessivamente
        }
        else
        {
            velocity += gravity * Time.deltaTime;
        }

        //Limita la velocità di caduta per prevenire gravità eccessiva
        if(velocity <=-5)
        {
            velocity = -5;
        }

        //Attiva il trail se il dash è attivo
        if(_state == PlayerState.dash)
            dashTrail.enabled = true;
        else
            dashTrail.enabled = false;
    }

    void AnimationHandler()
    {
        switch (_state)
        {
            case PlayerState.idle:
                anim.SetBool("jump", false);
                anim.SetBool("damaged", false);
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
                anim.SetBool("damaged", true);
                break;
            case PlayerState.dead:
                anim.SetBool("death", true);
                break;
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        if(hit.gameObject.tag == "Nemico")
        {
            _state = PlayerState.damage;
        }
    }

}
