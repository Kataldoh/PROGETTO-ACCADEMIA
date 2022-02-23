  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainPlayerScript : MonoBehaviour
{
    public static MainPlayerScript pInstance;   //creo un'istanza del player
    

    [Header("Player Data")]
    //*****************************************
    public PlayerData pdata; // SCRIPTABLE OBJECT che determina forza del salto,velocit� della rotazione,lunghezza del raycast frontale
    public WeaponStats[] weapons_SO;            //contiene gli stats delle armi (SCRIPTABLE OBJECTS)
    [SerializeField] GameObject[] projectiles;  //contiene i proiettili da sparare
    public PlayerState _state;              // Stati del player
    [SerializeField] PlayerStatesEvents _Estates;
    [SerializeField] public Vector3 move;
    [SerializeField] TrailRenderer dashTrail;
    public float speed;
    public CharacterController controller;
    public float dirX;
    public int dir;                     //direzione nella quale il character si gira
    public float height;                //altezza del CharacterController
    public float hangTime;              //(da implementare) un hangtime per dare al giocatore una finestra per saltare dopo essere in aria
    public float dashRechargeTime;      //variabile utilizzata per il controllo del tempo di ricarica del dash
    public float dashTimer;             //variabile contenente il timer del dash
    public float invincibilityTimer;             //variabile contenente il timer del dash
    public float invincibilityDuration;             //variabile contenente il timer del dash

    [SerializeField] GameObject mesh;
    
    [Header("Various Checks")]
    [SerializeField] public bool isGrounded; //bool che determina se il player è a terra oppure no
    public bool isJump;
    public bool isDash;
    public bool isSprinting;
    public bool isInvincible;  

    [Header("Unlocked Abilities")]
    public bool dashUnlocked;
    public bool rollUnlocked;

    [Header("Assigned Variables")]
    [SerializeField] Transform foot;        //posizione del "piede" del player, dove la sfera per trovare se si è a terra sarà situata
    [SerializeField] Transform rayhead;
    [SerializeField] Transform rollCheck;
    [SerializeField] public LayerMask layer,shootingIgnoreLayer;
    public Animator anim;
    public LineRenderer laserRender;    
    public GameObject endSpark;

    [Header("Jump Physics variables")]
    [SerializeField] float JumpForce;       //forza del salto
    [SerializeField] public float gravity;
    [SerializeField] public float weight;   //peso a terra del player
    [SerializeField] float radLenght;       //valore del raggio della sfera ai piedi del player per controllare se è a terra
    [SerializeField] public float velocity;

    //*****************************************

    [Header("Jump Physics variables")]
    float rot=90;
    float startingZ;
    WeaponMethods aM;                   //variabile usata per utilizzare i metodi delle armi
    
    public Vector3 lastShotPosition;    //contiene l'ultima posizione sparata dal player
    

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
        if(isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if(invincibilityTimer >= invincibilityDuration)
            {
                invincibilityTimer = 0;
                isInvincible = false;
            }
        }

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

        //impostazione del timer del dash
        if(dashTimer < dashRechargeTime)
            dashTimer += Time.deltaTime;
        else
            dashTimer = dashRechargeTime;

        //esegue il dash se il timer è maggiore/uguale a quello presentato in dashRechrgeTime
        if(Input.GetButtonDown("Fire3") && dashTimer >= dashRechargeTime)
            isDash = true;
        
        GameController.instance.BarraStamina.SetStamina(dashTimer * 100);
        aM.ScreenAiming(rayhead, shootingIgnoreLayer);
        aM.GeneralWeaponHandler(weapons_SO[0], rayhead, projectiles, shootingIgnoreLayer);


        /*
        if (!isSprinting)
        {
            GameController.instance.RegenStamina();
          
        }

        if (isSprinting)
        {
            GameController.instance.TakeStamina();
        }
        */
        if(Input.GetKey(KeyCode.O))
        {
            SavePlayer();
        }
        if (Input.GetKey(KeyCode.L))
        {
            LoadPlayer();
        }

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

        if(isInvincible)
        {
            if(mesh.activeSelf)
            {
                mesh.SetActive(false);
            }
            else
            {
                mesh.SetActive(true);
            }
        }
        else
        {
            mesh.SetActive(true);
        }
    }

    //***************************//
    //          METODI
    //***************************//

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
            /*
            case PlayerState.sprinting:
                _Estates.P_Sprinting();
                break;
            */
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
        if(move.y > 0 || !rollUnlocked)
        {
            move.y=0;
        }

        bool isStuck = RollCheck();

        if (isStuck)
        {
            move.y = -1;
        }
            
        //Assegno il metodo per controllare se si è a terra ad una variabile
        isGrounded = IsGrounded();

        print(dirX);
        //determino la direzione nel quale il player guarda
        if (move.x > 0 || (move.x == 0 && dirX > 0))
        {
            rot = 90;
            dir = 1;
        }
        else if (move.x < 0 || (move.x == 0 && dirX < 0))
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
            if(_state == PlayerState.damage)
                velocity = 0;
            else
                velocity = weight;        //Resetta la velocity dandogli un peso
            isJump = false;               //mette a falso la variabile di salto per permettere di saltare sucessivamente
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

        if(_state != PlayerState.damage)
            controller.detectCollisions = true;
    }

    //metodo di controllo delle animazioni
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
                /*
            case PlayerState.sprinting:
                anim.SetBool("jump", false);
                anim.SetFloat("posx", move.x, 0.05f, Time.deltaTime);
                anim.SetFloat("posy", move.y, 0.15f, Time.deltaTime);
                break;
                */
        }
    }

    bool RollCheck()
    {
        return Physics.CheckSphere(rollCheck.position, radLenght, layer);
    }

    //metodo di controllo del terreno
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

    //Collisioni
    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        if(hit.gameObject.tag == "Nemico" || hit.gameObject.tag == "DamageDealer" 
                        && _state != PlayerState.damage)
        {
            if(!isInvincible)
                _state = PlayerState.damage;
        }

        if(hit.gameObject.tag == "Gateway")
        {
            SceneManager.LoadScene(2);
        }

        if(hit.gameObject.tag == "Pickup")
        {
            int unlockAbilityN = hit.gameObject.GetComponent<UpgradePickups>().powerUpValue;

            switch(unlockAbilityN)
            {
                case 0:
                    dashUnlocked = true;
                    break;
                case 1:
                    rollUnlocked = true;
                    break;
            }


            Destroy(hit.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Nemico" || other.gameObject.tag == "DamageDealer" && _state != PlayerState.damage && !isInvincible)
        {
            if(!isInvincible)
                _state = PlayerState.damage;
        }

        if(other.gameObject.layer == 10)
        {
            GameController g = GameController.instance;

            g.cameraOffset = other.GetComponent<CameraSettingsTrigger>().offset;
            g.followPlayerX = other.GetComponent<CameraSettingsTrigger>().followPlayerX;
            g.followPlayerY = other.GetComponent<CameraSettingsTrigger>().followPlayerY;
            g.cameraSmoothing = other.GetComponent<CameraSettingsTrigger>().smoothDamp;
            g.triggerPos = other.transform.position;
        }
    }

    //Loadand Save System
    public void SavePlayer()
    {
        SaveSystem.SaveDataPlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerDataforSave data = SaveSystem.LoadPlayer();
        Vector3 playerposition;
        playerposition.x = data.playerposition[0];
        playerposition.y = data.playerposition[1];
        playerposition.z = data.playerposition[2];
        transform.position = playerposition;
    }

}
