using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayerScript : MonoBehaviour
{
    public static MainPlayerScript pInstance;   //creo un'istanza del player

    public PlayerData pdata; // SCRIPTABLE OBJECT che determina forza del salto,velocit� della rotazione,lunghezza del raycast frontale

    public PlayerState _state; // Stati del player
    [SerializeField] PlayerStatesEvents _Estates;

    [SerializeField] public Vector3 move;
    public CharacterController controller;
    [SerializeField] public bool isGrounded; //bool che determina se il player � a terra oppure no
    public bool isJump;
    [SerializeField] Transform foot;    //posizione del "piede" del player, dove la sfera per trovare se si è a terra sarà situata
    [SerializeField] Transform rayhead;
    [SerializeField] public LayerMask layer;
    [SerializeField] public Animator anim;

    //*****************************************
    [SerializeField] float JumpForce;   //forza del salto
    [SerializeField] public float gravity;
    [SerializeField] public float weight;   //peso a terra del player
    [SerializeField] float radLenght;       //valore del raggio della sfera ai piedi del player per controllare se è a terra
    [SerializeField] public Transform cursor;   //posizione del cursore
    [SerializeField] public float velocity;
    float rot=90;
    public int dir;     //direzione nella quale il character si gira
    float startingZ;
    LineRenderer laserRender;
    
    private void Awake()
    {
        pInstance = this;   //Assegno l'istanza di questo player
    }
    void Start()
    {
        _Estates = new PlayerStatesEvents();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        laserRender = GetComponent<LineRenderer>();
        startingZ = transform.position.z;
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

        //Questo if serve solo per testare se il metodo TakeDamage funziona (Aldo)
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameController.instance.TakeDamage(20);
        }
        */
    }


    void FixedUpdate()
    {
        //Il player non eseguirà alcuna azione se lo stato non è in play
        if (GameController.instance._state == GameState.play)
        {

            StateIndipendentActions();  //metodo per la gestione di azioni indipendenti dagli stati
            AnimationHandler(); //metodo per la gestione delle animazioni
            States();       //metodo per la gestione degli stati


            //-----------------
            //TEST PER LO SPARO
            //-----------------

            //calcolo della direzione dello sparo
            Vector3 direction = new Vector3(-(transform.position.x - cursor.position.x), -(transform.position.y - cursor.position.y), 0).normalized;

            //(transform.position - cursor.position).normalized;
            //print(transform.position - cursor.position);
            //var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            Debug.DrawRay(rayhead.position, direction * 5, Color.red);  //disegno il raggio nella scena

            //(Non funzionale/Chiedere a Fabrizio) Come disegno un raggio da punto A a punto B avendo calcolato la direzione del raggio 
            laserRender.SetPosition(0, rayhead.position);
            laserRender.SetPosition(1, direction);

            //Se tengo premuto tasto destro del mouse, si inizierà a "sparare"
            if (Input.GetButton("Fire2"))
            {
                laserRender.enabled = true;
                RaycastHit hit;
                if (Physics.Raycast(rayhead.position, direction, out hit, 5))   //Se il raycast colpisce qualcosa 
                {
                    laserRender.SetPosition(1, hit.point);         //Disegna la fine del raggio sul punto colpito
                    print("Hit");
                    if(hit.collider.tag == "Nemico")            //Se colpisce un nemico
                    {
                        print("Hit Enemy");
                        Destroy(hit.collider.gameObject);
                    }
                        
                }
            }
            else
            {
                laserRender.enabled = false;
            }

        }
    }

    
    public bool IsGrounded()
    {
        return Physics.CheckSphere(foot.position, radLenght, layer);
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

        //Registra il movimento sugli assi
        move = new Vector3(
                   Input.GetAxis("Horizontal") * Time.deltaTime * 64,
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

    
    bool IsInRayCastDireciton(Vector3 direction, float lenght, LayerMask layer) {
        Debug.DrawRay(rayhead.position, direction * lenght, Color.red);
        return Physics.Raycast(rayhead.position, direction, out RaycastHit hit, lenght, layer);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        if(hit.gameObject.tag == "Nemico")
        {
            _state = PlayerState.damage;
        }
    }
    

}
