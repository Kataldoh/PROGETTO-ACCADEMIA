using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public EnemyData edata;
    public EnemyState _state;
    [SerializeField] HealthPlaceholder hp;
    CharacterController controller;
    [SerializeField] Animator anim;
    [SerializeField] Transform foot, turnAroundPoint;
    [SerializeField] Transform rayhead;
    [SerializeField] float rayLenght;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isJump;
    [SerializeField] float gravity;
    [SerializeField] float weight;
    [SerializeField] GameObject attackHitbox;
    float idleTimer, patrolTimer, attackTimer;
    float speedx;


    public Transform target;
    public float targetDistance;
    float playerEnemyDistance;

   
    Vector3 move;
    float velocity;
    // raccolta di elementi 
    [SerializeField] LayerMask layer;
    [SerializeField] LayerMask layer2;
    float direction;
    Quaternion qrot;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        direction = 1;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate() 
    {
        if (GameController.instance._state == GameState.play)
        {
            StatelessChecks();
            States();
        }
    }

    void States()
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
                Dead();
                break; 
        }
    }

    void Idle()
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

    void Patrol()
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

    void Attack()
    {
        idleTimer = 2.5f;
        patrolTimer = 0;
        float speedMultiplier = 0;

        direction = _vectorDir();

        if(playerEnemyDistance <= 0.5f)
        {
            print("Attack");
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

        if(playerEnemyDistance > targetDistance)
        {
            attackHitbox.SetActive(false);
            _state = EnemyState.idle;
        }
    }

    void Dead()
    {
        anim.SetBool("death", true);
        controller.enabled = false;
    }

    void StatelessChecks()
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

        controller.Move(transform.up * velocity * Time.deltaTime);
        /**/

        //controlla se il nemico arriva ad un muro, per poi girarsi
        if (IsInRayCastDireciton(transform.forward, 0.45f, layer, Color.green))
        {
            if (_state != EnemyState.attack)
                direction = direction * -1;
        }

        //controlla se il nemico arriva ad una sporgenza, per poi girarsi
        if(!Physics.CheckSphere(turnAroundPoint.position, 0.5f, layer))
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
    }

    bool IsInRayCastDireciton(Vector3 direction, float lenght, LayerMask layer, Color color)
    {
        Debug.DrawRay(rayhead.position, direction * lenght, color);
        return Physics.Raycast(rayhead.position, direction, out RaycastHit hit, lenght, layer);
    }
    
    bool IsGrounded()
    {
        return Physics.CheckSphere(foot.position, rayLenght, layer);
    }

    float _vectorDir() {
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
}
