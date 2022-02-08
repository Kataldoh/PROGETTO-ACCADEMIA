using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public EnemyData edata;
    public EnemyState _state;

    CharacterController controller;
    [SerializeField] Animator anim;
    [SerializeField] Transform foot;
    [SerializeField] Transform rayhead;
    [SerializeField] float rayLenght;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isJump;
    [SerializeField] float gravity;
    [SerializeField] float Wheight;


    public Transform target;
    public float distanza;
    public float movespeed;
    public float area;

   
    Vector3 move;
    float velocity;
    // raccolta di elementi 
    [SerializeField] LayerMask layer;
    [SerializeField] LayerMask layer2;
    float direction;
    Quaternion qrot;
    float startingZ;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        direction = 1;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        startingZ = transform.position.z;
    }
    // Update is called once per frame
    void Update()
    {
        
        if(transform.position.z != startingZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startingZ);
        }

        if (GameController.instance._state == GameState.play)
        {
            

            move = new Vector3(
             direction,
             0,
             0
            );

            isGrounded = IsGrounded();

            if (IsGrounded() && velocity < 0)
            {
                velocity = Wheight;
                anim.SetBool("jump", false);
                isJump = false;
            }
            else
            {
                velocity += gravity * Time.deltaTime;
            }


            if (IsInRayCastDireciton(transform.forward, 1, layer2, Color.red))
            {
                anim.SetBool("jump", true);
                isJump = true;
                velocity = edata.jumpForce;
            }

            if (IsInRayCastDireciton(transform.forward, 0.45f, layer, Color.green))
            {
                if (!isJump)
                    direction = direction * -1;
            }

            move.y = Mathf.Clamp(move.y, -1, 0);
            anim.SetFloat("posx", move.x, 0.05f, Time.deltaTime);
           // controller.Move(move * edata.force * Time.deltaTime);
            controller.Move(transform.up * velocity * Time.deltaTime);
        }


        AttackEnemy();

        //rotazione del nemico

        transform.rotation = qrot;


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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            GameController.instance._state = GameState.dead;
        }
    }


    public void AttackEnemy()
    {
        distanza = Vector3.Distance(target.position, transform.position);

        if (distanza <= 3.5f)
        {
            print(_vectorDir());
            qrot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90 * _vectorDir(), 0), Time.deltaTime * edata.speedRot);
            controller.Move(move * edata.force * Time.deltaTime * _vectorDir());
        }
        else {
            qrot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90 * direction, 0), Time.deltaTime * edata.speedRot);
            controller.Move(move * edata.force * Time.deltaTime);
        }
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
}
