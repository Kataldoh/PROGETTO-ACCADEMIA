using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCode : MonoBehaviour
{

    [SerializeField] BossState bState;
    public Transform groundCheck, bumpCheck;
    public float radLenght;
    public LayerMask layer;
    public float idleTimer, jumpForce;
    GameObject target;
    int rot, moveDir;
    Vector3 move;
    CharacterController controller;
    Animator anim;
    float velocity;
    public bool jumped;
    public GameObject weakPoint;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        controller= GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(weakPoint == null)
        {
            bState=BossState.dead;
        }

        move = new Vector3(moveDir, 0, 0);

        switch (bState)
        {
            case BossState.idle:
                Idle();
                break;
            case BossState.attack:
                Attack();
                break;
            case BossState.jump:
                Jump();
                break;
            case BossState.dead:
                Death();
                break;
        }
    }

    void Idle()
    {
        idleTimer += Time.deltaTime;
        anim.SetBool("Grounded", true);
        anim.SetBool("HasBumped", false);
        anim.SetBool("Running", false);
        jumped= false;
        velocity = 0;

        if(transform.position.x < target.transform.position.x)
        {
            rot = 90;
            moveDir = 1;
        }
        else
        {
            rot = 270;
            moveDir = -1;
        }

        Quaternion qrot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rot, 0), Time.deltaTime * 5);
        transform.rotation = qrot;

        if(idleTimer >= 5)
        {
            if(Random.Range(0,2) == 0)
            {
                bState = BossState.attack;
            }
            else
            {
                bState = BossState.jump;
                velocity = 6;
            }
            idleTimer = 0;
        }
    }

    void Attack()
    {
        anim.SetBool("Running", true);
        controller.Move(move * 5 * Time.deltaTime);
        if(HasBumped())
        {
            bState = BossState.idle;
            anim.SetBool("HasBumped", true);
        }
    }

    void Jump()
    {
        anim.SetBool("Grounded", false);
        float jumpMoveSpeed;

        if(IsGrounded())
        {
            if(velocity < 0)
                bState = BossState.idle;
        }
        else
        {
            velocity += -9.81f * Time.deltaTime;
        }

        if(HasBumped())
        {
            jumpMoveSpeed = 0;
            anim.SetBool("HasBumped", true);
        }
        else
        {
            jumpMoveSpeed = 5;
        }

        controller.Move(move * jumpMoveSpeed * Time.deltaTime);
        controller.Move(transform.up * velocity * Time.deltaTime);

    }

    void Death()
    {
        Destroy(this.gameObject);
    }

    public bool HasBumped()
    {
        return Physics.CheckSphere(bumpCheck.position, radLenght, layer);
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, radLenght, layer);
    }
}
