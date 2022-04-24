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
    public WeaponStats[] weapons_SO;                //contiene gli stats delle armi (SCRIPTABLE OBJECTS)
    [SerializeField] GameObject[] projectiles;      //contiene i proiettili da sparare
    public PlayerState _state;                      // Stati del player
    [SerializeField] PlayerStatesEvents _Estates;
    [SerializeField] public Vector3 move;           //Vector3 che contiene gli input di movimento
    [SerializeField] TrailRenderer dashTrail;       
    public float speed;                             //velocità del player
    public CharacterController controller;
    public float dirX;                  
    public int dir;                     //direzione nella quale il character si gira
    public float height;                //altezza del CharacterController
    public float hangTime;              //un timer per dare al giocatore una finestra di tempo per saltare dopo essere in aria
    public float dashRechargeTime;      //variabile utilizzata per il controllo del tempo di ricarica del dash
    public float dashTimer;             //variabile contenente il timer del dash
    public float invincibilityTimer;             //variabile contenente il timer del dash
    public float invincibilityDuration;             //variabile contenente il timer del dash

    [SerializeField] GameObject mesh;
    
    [Header("Various Checks")]
    [SerializeField] public bool isGrounded;    //bool che determina se il player è a terra oppure no
    public bool isJump;                         //se il player salta
    public bool isSliding;                      //se il player fa lo slide
    public bool isDash;                         //se il player esegue il dash
    public bool isInvincible;                   //se il player è invincibile
    public bool hasSomethingAbove;
    public bool hasSomethingInFront;
    public bool invertRotation;

    [Header("Unlocked Abilities")]
    public bool dashUnlocked;                   //bool che determina che il dash sia sbloccato
    public bool rollUnlocked;                   //se il roll/crouch sia sbloccato

    [SerializeField] GameObject[] powerups;                 //Testi che compaiono entrato nell'evento
    public int n=0;                                        //valore array
    bool sceneschange;

    [Header("Assigned Variables")]
    public AnimationCurve jumpArc;
    public AnimationCurve gravityArc;
    [SerializeField] Transform[] groundCheck;        //posizione del "piede" del player, dove la sfera per trovare se si è a terra sarà situata
    [SerializeField] Transform rayhead;              //posizione dal quale la mira e lo sparo parte, messo
    [SerializeField] Transform rollCheck;            //posizione dalla quale si controlla se il player rimane abbassato
    [SerializeField] Transform wallCheck;            //posizione dalla quale si controlla se il player è contro un muro
    [SerializeField] public LayerMask layer,shootingIgnoreLayer;    
    public Animator anim;
    public LineRenderer laserRender;    
    public GameObject endSpark;

    [Header("Jump Physics variables")]
    [SerializeField] float JumpForce;       //forza del salto
    [SerializeField] public float gravity;  //variabile della gravità
    [SerializeField] public float weight;   //peso a terra del player
    [SerializeField] public float velocity;

    [Header("Ray Lenghts variables")]
    [SerializeField] float radLenght;       //valore della lunghezza dei raycast ai piedi del player per controllare se è a terra
    [SerializeField] float rollCheckLenght; //valore della lunghezza del raycast per controllare se ha qualcosa sopra di se
    [SerializeField] float wallCheckLenght; //valore della lunghezza del raycast per controllare se ha qualcosa sopra di se


    //*****************************************

    [Header("Misc checks")]
    
    float rot=90;
    float rotInvertion = 0;
    float startingZ;
    WeaponMethods aM;                   //variabile usata per utilizzare i metodi delle armi
    
    public Vector3 lastShotPosition;    //contiene l'ultima posizione sparata dal player
    

    private void Awake()
    {
        pInstance = this;   //Assegno l'istanza di questo player
    }


    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Respawn") != null)
            transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;

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
        if (GameController.instance._state == GameState.play)
        {
            StateIndipendentActionsUPDATE();
        }
            
    }


    void FixedUpdate()
    {
        //Il player non eseguirà alcuna azione se lo stato non è in play
        if (GameController.instance._state == GameState.play)
        {
            States();       //metodo per la gestione degli stati
            StateIndipendentActionsFIXED_UPDATE();  //metodo per la gestione di azioni indipendenti dagli stati
            AnimationHandler(); //metodo per la gestione delle animazioni

            if (GameObject.Find("Powerups"))
            {
                Powerups();     //metodo per far comparire i power UP, disattivato se non ce ne sono
                GameController.instance.eventpowerup = true;
            }
            else
            {
                GameController.instance.eventpowerup = false;
            }
            
        }

        //Fà sfarfallare la texture quando si è invincibili
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
        else    //al termine dell'invincibilità si riattiva la mesh
        {
            mesh.SetActive(true);
        }
    }

    //***************************//
    //          METODI
    //***************************//

    //--------------------
    //GESTIONE DEGLI STATI
    //--------------------
    public void States() {
        switch (_state)
        {
            case PlayerState.idle:
                _Estates.P_Idle();
                break;
            case PlayerState.groundMoving:
                if (!hasSomethingInFront)
                {
                    SoundManager.PlaySound(SoundManager.Sound.PlayerSteps);

                }
                _Estates.P_Move();
                break;
            case PlayerState.jump:
                _Estates.P_Jump();
                break;
            case PlayerState.walljump:
                _Estates.P_WallJump();
                break;
            case PlayerState.dash:

                SoundManager.PlaySound(SoundManager.Sound.Dashing);


                _Estates.P_Dash();
                break;
            case PlayerState.sliding:
                _Estates.P_Slide();
                break;
            case PlayerState.damage:

                SoundManager.PlaySound(SoundManager.Sound.PlayerHit);


                _Estates.P_Damage();
                break;
            case PlayerState.dead:
                _Estates.P_Death();
                break;
        }
    }

    //---------------------------------------------
    //GESTIONE DI ELEMENTI INDIPENDENTI DAGLI STATI
    //---------------------------------------------
    public void StateIndipendentActionsFIXED_UPDATE()
    {
        //--------------------
        //Registra il movimento sugli assi
        move = new Vector3(
                   Input.GetAxis("Horizontal") * Time.deltaTime * speed,
                   Input.GetAxis("Vertical"),
                   0
                );
        //--------------------

        

        //--------------------
        //Mette a zero move.y per prevenire salti più alti ed errori d'animazione
        if (move.y > 0 || !rollUnlocked)
        {
            move.y=0;
        }
        //--------------------

        //--------------------
        //Controlla se il player ha qualcosa sopra di sè
        hasSomethingAbove = RollCheck();
        Debug.DrawRay(rollCheck.position, transform.up * rollCheckLenght, Color.red);
        //E lo forza ad abbassarsi
        if (hasSomethingAbove)
        {
            move.y = -1;
        }
        //--------------------

        //--------------------
        //Assegno il metodo per controllare se si è a terra ad una variabile
        isGrounded = IsGrounded();
        //--------------------

        //--------------------
        //determino la direzione nel quale il player guarda
        if (invertRotation)
            rotInvertion = -1;
        else
            rotInvertion = 1;

        if (move.x > 0 || (move.x == 0 && dirX > 0))
        {
            rot = 90 * rotInvertion;
            dir = 1;
        }
        else if (move.x < 0 || (move.x == 0 && dirX < 0))
        {
            rot = -90 * rotInvertion;
            dir = -1;
        }
        //--------------------

        //--------------------
        //rotazione del player
        Quaternion qrot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rot, 0), Time.deltaTime * pdata.speedRot);
        transform.rotation = qrot;
        //--------------------

        //--------------------
        //Applico Gravità
        if (IsGrounded() && velocity <= 0)  //Se a terra con velocity <= 0
        {

            if (_state == PlayerState.damage)
                velocity = 0;
            else
                velocity = weight;        //Resetta la velocity dandogli un peso
            isJump = false;               //mette a falso la variabile di salto per permettere di saltare sucessivamente
        }
        else
        {
            velocity += gravity * gravityArc.Evaluate(-Time.deltaTime * gravity / 2);
        }

        //Limita la velocità di caduta per prevenire gravità eccessiva
        if (velocity <=-10)
        {
            velocity = -10;
        }
        //--------------------

        //--------------------
        //Attiva il trail se il dash è attivo
        if (_state == PlayerState.dash)
            dashTrail.enabled = true;
        else
        {
            dashTrail.GetComponent<TrailRenderer>().Clear();
            dashTrail.enabled = false;
        }
            

        if(_state != PlayerState.damage)
            controller.detectCollisions = true;
        //--------------------
    }

    public void StateIndipendentActionsUPDATE()
    {
        //controlla il tempo di invincibilità
        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer >= invincibilityDuration)
            {
                invincibilityTimer = 0;
                isInvincible = false;
            }
        }

        hasSomethingInFront = WallCheck();

        //Mantiene la posizione della Z costante a quella iniziale
        if (transform.position.z != startingZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startingZ);
        }

        //Se la vita del player è a 0
        if (GameController.instance.CurrentHealth == 0)
        {
            _state = PlayerState.dead;
            GameController.instance._state = GameState.dead;
        }

        //--------------------
        //GESTIONE DEL DASH
        //--------------------
        //impostazione del timer del dash
        if (dashTimer < dashRechargeTime)
            dashTimer += Time.deltaTime;
        else
            dashTimer = dashRechargeTime;

        //esegue il dash se il timer è maggiore/uguale a quello presentato in dashRechrgeTime (se sbloccato)
        if (Input.GetButtonDown("Fire3") && dashUnlocked && dashTimer >= dashRechargeTime && !hasSomethingInFront)
            isDash = true;

        //Setta la barra della stamina tramite il timer del dash
        GameController.instance.BarraStamina.SetStamina(dashTimer * 100);

        //(PROVVISORIO)
        //Controlla se si può fare il dash, quando si è a terra e ci si può muovere se il roll è sbloccato
        if (Input.GetButtonDown("SlideButton"))
            if (_state == PlayerState.groundMoving && rollUnlocked && !hasSomethingInFront)
                _state = PlayerState.sliding;

        //---------------------------
        //GESTIONE DELLA MIRA E SPARO
        //---------------------------
        aM.GeneralWeaponHandler(weapons_SO[0], rayhead, endSpark, projectiles, shootingIgnoreLayer);


        //Sistema di salvataggio
        if (Input.GetKey(KeyCode.O))
        {
            SavePlayer();
        }
        if (Input.GetKey(KeyCode.L))
        {
            LoadPlayer();
        }
    }

    //-------------------------------------------------------
    //METODO DI CONTROLLO DELLE ANIMAZIONI IN BASE AGLI STATI
    //-------------------------------------------------------
    void AnimationHandler()
    {
        float x_MovementFloat = move.x;
        if (hasSomethingInFront)
            x_MovementFloat = 0;

        switch (_state)
        {
            case PlayerState.idle:
                anim.SetBool("jump", false);
                anim.SetBool("damaged", false);
                anim.SetBool("sliding", false);
                anim.SetBool("dash", false);
                anim.SetFloat("posx", 0, 0.05f, Time.deltaTime);
                anim.SetFloat("posy", move.y, 0.15f, Time.deltaTime);
                anim.SetFloat("velocity", 0, 0, Time.deltaTime);
                break;
            case PlayerState.groundMoving:
                anim.SetBool("jump", false);
                anim.SetBool("sliding", false);
                anim.SetBool("dash", false);
                anim.SetFloat("posx", x_MovementFloat, 0.05f, Time.deltaTime);
                anim.SetFloat("posy", move.y, 0.15f, Time.deltaTime);
                anim.SetFloat("velocity", 0, 0, Time.deltaTime);
                break;
            case PlayerState.dash:
                anim.SetBool("dash", true);
                break;
            case PlayerState.sliding:
                anim.SetBool("sliding", true);
                break;
            case PlayerState.jump:
                anim.SetBool("jump", true);
                anim.SetBool("dash", false);
                anim.SetFloat("velocity", velocity, 0, Time.deltaTime);
                anim.SetBool("sliding", false);
                break;
            case PlayerState.damage:
                anim.SetBool("damaged", true);
                break;
            case PlayerState.dead:
                anim.SetBool("death", true);
                break;
        }

        
    }

    //Controlla se il player ha qualcosa al disopra di se
    bool RollCheck()
    {
        return Physics.Raycast(rollCheck.position, transform.up, rollCheckLenght, layer);
    }

    //Controlla se il player ha qualcosa al disopra di se
    bool WallCheck()
    {
        return Physics.Raycast(wallCheck.position, transform.forward, wallCheckLenght, layer);
    }

    //metodo di controllo del terreno
    public bool IsGrounded()
    {
     
        //Se i 2 raycast posti a destra e a sinistra del player non trovano un oggetto nel layer Ground
        if (!Physics.Raycast(groundCheck[0].position,-transform.up, radLenght, layer) && 
                !Physics.Raycast(groundCheck[1].position, -transform.up, radLenght, layer))
        {
            //inizio ad aggiungere tempo all'hangTime
            //un timer per dare al giocatore una finestra di tempo per saltare dopo essere in aria
            hangTime += Time.deltaTime; 

            if(hangTime >= 0.35f)    //se l'hangtime è maggiore del valore dato ritorna falso, quindi il player non è a terra
            {
                return false;
            }
            else                    //altrimenti è a terra
            {
                return true;
            }  
        }
        else                        //altrimenti è a terra e l'hangtime è messo a 0
        {

            hangTime = 0;
            return true;
        }

        
    }

    //Collisioni
    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        //Se si entra in contatto con un gameobject con tag "Nemico" o "Damage" quando il player non è già nello stato di danno
        if(hit.gameObject.tag == "Nemico" || hit.gameObject.tag == "DamageDealer" 
                        && _state != PlayerState.damage)
        {
            if(!isInvincible)                   //ed eseguendo un'ultimo controllo per vedere se ha ancora invincibile
                _state = PlayerState.damage;    //setta lo stato di danno
        }

        //(PROVVISORIO)
        //Se si entra in contatto con un gameobject con tag "Gateway" si accede alla scena del Boss
        if (hit.gameObject.tag == "Gateway")
        {
            SceneManager.LoadScene(2);  
        }

        //--------------------
        //Gestione dei pickup 
        if(hit.gameObject.tag == "Pickup")
        {
            SoundManager.PlaySound(SoundManager.Sound.PowerUp);

            //Prende dal codice assegnato ai pickup il valore assegnato che indica il tipo
            int unlockAbilityN = hit.gameObject.GetComponent<UpgradePickups>().powerUpValue;

            //Switch che controlla cosa sbloccare dato il valore del pickup
            //------------------------
            // 0: Dash
            // 1: Roll/Crouch
            //------------------------
            switch(unlockAbilityN)
            {
                case 0:
                    dashUnlocked = true;
                    //compaiono i testi evento 1 
                    GameController.instance._state = GameState.eventpausa;
                    //powerups[0].SetActive(true);
                    n = 0;
                    break;
                case 1:
                    rollUnlocked = true;
                    //compaiono i testi evento 2
                    GameController.instance._state = GameState.eventpausa;
                    //powerups[1].SetActive(true);
                    n = 1;
                    break;
            }

            //Distruggi il powerup una volta preso
            Destroy(hit.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        //Se si entra in contatto con un trigger con tag "Nemico" o "Damage" quando il player non è già nello stato di danno
        if (other.gameObject.tag == "Nemico" || other.gameObject.tag == "DamageDealer" && _state != PlayerState.damage && !isInvincible)
        {
            if(!isInvincible)                   //ed eseguendo un'ultimo controllo per vedere se ha ancora invincibile
                _state = PlayerState.damage;    //setta lo stato di danno
        }

        //Se si entra in contatto con un trigger presente del layer 10, designato ai SETTAGGI DELLA CAMERA
        //Assegna i vari valori della telecamera alle rispettive variabili presenti nel GameController
        if (other.gameObject.layer == 10)
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

    public void Powerups()
    { 
        powerups[n].SetActive(false);       //Disattiva i powerup text

        if (n > 1) //check per non farlo andare oltre l' array 1 è da modificare nel caso in cui il valore massimo di array cambi
        {
            n = 0;
        }
    }
}
