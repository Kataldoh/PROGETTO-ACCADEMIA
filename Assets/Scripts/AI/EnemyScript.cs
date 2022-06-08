using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public EnemyData edata;
    public EnemyState _state;
    [SerializeField] HealthPlaceholder hp;
    public CharacterController controller;
    [SerializeField] Animator anim;
    [SerializeField] Transform foot, turnAroundPoint;
    [SerializeField] Transform rayhead;
    [SerializeField] float rayLenght;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isJump, hasGameController;
    [SerializeField] float gravity;
    [SerializeField] float weight;
    [SerializeField] GameObject attackHitbox;
    [SerializeField] GameObject spawnOnDeath;
    float idleTimer, patrolTimer, attackTimer;
    float speedx;
    public bool isDead;
    Collider collider;
    public Transform target;
    public float targetDistance;
    float playerEnemyDistance;
    Vector3 move;
    float velocity;
    // raccolta di elementi 
    [SerializeField] public LayerMask layer;
    float direction;
    Quaternion qrot;

    private void Start()
    {
        isDead = false;
        if (GetComponent<CharacterController>() != null)
            controller = GetComponent<CharacterController>();
        if (GetComponent<Animator>() != null)
            anim = GetComponent<Animator>();

        //transform.Rotate(0, 90, 0);
        collider = GetComponent<Collider>();
        direction = 1;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Ragdoll Rigidboy&Collider

        //setRigidbodyState(true);
        //setColliderState(false);
    }

    private void FixedUpdate() 
    {
        if (GameController.instance._state == GameState.play)
        {
            StatelessChecks();
            States();
            
        }
    }

    private void Update()
    {
        if (GameController.instance._state == GameState.play)
        {
            StatesNOPhys();
        }
    }

    public virtual void States()
    {
        switch(_state)
        {
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.attack:
                Attack();
                break;
            case EnemyState.patrol:
                Patrol();
                break; 
            case EnemyState.dead:
                if(!isDead)
                    Dead();
                break; 
        }
    }
    public virtual void StatesNOPhys()
    {

    }

    public virtual void Idle()
    {
        patrolTimer=0;
        idleTimer += Time.deltaTime;
        speedx = 0;
        attackHitbox.SetActive(false);

        if(idleTimer >= Random.Range(4, 6))
        {
            _state = EnemyState.patrol;
        }
    }

    public virtual void Patrol()
    {
        idleTimer = 0;
        patrolTimer += Time.deltaTime;
        move = new Vector3(
            direction,
            0,
            0
        );

        speedx = Vector3.Dot(move * edata.force, transform.forward);

        controller.Move(move * edata.force * Time.deltaTime);

        if(patrolTimer >= Random.Range(6, 12))
        {
            _state = EnemyState.idle;
        }

        if(playerEnemyDistance <= targetDistance)
        {
            _state = EnemyState.attack;
        }
    }

    public virtual void Attack()
    {
        idleTimer = 2.5f;
        patrolTimer = 0;
        float speedMultiplier = 0;

        direction = _vectorDir();

        if(playerEnemyDistance <= 0.5f && IsGrounded())
        {
            speedMultiplier = 0;
            speedx = 0;
            attackHitbox.SetActive(true);
            _state = EnemyState.idle;
        }
        else
        {
            speedMultiplier = 2.5f;
            speedx = Vector3.Dot(move * edata.force * speedMultiplier, transform.forward);
            attackHitbox.SetActive(false);
        }

        controller.Move(move * edata.force * speedMultiplier * Time.deltaTime);

        if(playerEnemyDistance > targetDistance && IsGrounded())
        {
            attackHitbox.SetActive(false);
            _state = EnemyState.idle;
        }
    }

    public virtual void Dead()
    {
        isDead = true;
        SoundManager.PlaySound(SoundManager.Sound.EnemyDie);
        anim.SetBool("death", true);

        Vector3 healthSpawnPos;
        if (GameController.instance.CurrentHealth < 100)
        {
            healthSpawnPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Instantiate(spawnOnDeath, healthSpawnPos, spawnOnDeath.transform.rotation);
        }
        
        collider.enabled = false;
        if (controller != null)
        {
            controller.detectCollisions = false;
            controller.enabled = false;
        }

        //      !!!!!!RAGDOLL!!!
        Destroy(gameObject, 3f);//distruggo il gameObject dopo 3 secondi
        GetComponent<Animator>().enabled = false; //disabilito l'animator
        setRigidbodyState(false);//richiamo il metodo Rigidobody
        setColliderState(true);//richiamo il metodo Collider

    }

    public virtual void StatelessChecks()
    {
        playerEnemyDistance = Vector3.Distance(transform.position, target.transform.position);
        isGrounded = IsGrounded();
        
        //controllo la velocità dell'animazione;
        anim.SetFloat("posx", speedx, 0.2f, Time.deltaTime);

        //controllo della gravità
        if (IsGrounded() && velocity < 0)
        {
            velocity = weight;
        }
        else
        {
            velocity += gravity * Time.deltaTime;
        }

        if(!isDead)
            controller.Move(transform.up * velocity * Time.deltaTime);
        /**/

        //controlla se il nemico arriva ad un muro, per poi girarsi
        if (IsInRayCastDireciton(transform.forward, 0.45f, layer, Color.green))
        {
            if (_state != EnemyState.attack)
                direction = direction * -1;
        }

        //controlla se il nemico arriva ad una sporgenza, per poi girarsi
        if(!Physics.Raycast(turnAroundPoint.position, -transform.up, 0.3f, layer))
        {
            if (_state != EnemyState.attack)
                direction = direction * -1;
        }

        move.y = Mathf.Clamp(move.y, -1, 0);

        //rotazione del nemico
        if(_state != EnemyState.dead)
            qrot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90 * direction, 0), Time.deltaTime * edata.speedRot);
        transform.rotation = qrot;

        if(hp.health <= 0)
        {
            _state = EnemyState.dead;
        }

        if (!isDead)
        {
            controller.enabled = true;
            GetComponent<Collider>().enabled = true;
        }
        else
        {
            controller.enabled = false;
            GetComponent<Collider>().enabled = false;
        }
            
    }

    public bool IsInRayCastDireciton(Vector3 direction, float lenght, LayerMask layer, Color color)
    {
        Debug.DrawRay(rayhead.position, direction * lenght, color);
        return Physics.Raycast(rayhead.position, direction, out RaycastHit hit, lenght, layer);
    }
    
    public bool IsGrounded()
    {
        return Physics.CheckSphere(foot.position, rayLenght, layer);
    }

    public float _vectorDir() {
        var vettoredir = (target.position - transform.position).normalized;
        var dist = vettoredir.magnitude;
        Vector3 direction = (vettoredir / dist);
        if (Mathf.Round(direction.x) == -1) {
            return -1;
        }
        else {
            return 1;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        if(hit.gameObject.tag == "Nemico")
        {
            direction *= -1;
        }
    }

    //RAGDOLL

    //abilito lo stato dei rigidbodies
    void setRigidbodyState(bool state)
    {
        //Decido per ogni componente del collider quando abilitarlo/disabilitarlo
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        controller.enabled = !state;
    }

    //abilito lo stato dei colliders
    void setColliderState(bool state)
    {
        //Decido per ogni componente del collider quando abilitarlo/disabilitarlo
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach(Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

}
