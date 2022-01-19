using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector3 move;
    float velocity;
    [SerializeField] LayerMask layer;
    [SerializeField] LayerMask layer2;
    float direction;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        direction = 1;
    }
    // Update is called once per frame
    void Update()
    {

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


            //rotazione del nemico
            Quaternion qrot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90 * direction, 0), Time.deltaTime * edata.speedRot);
            transform.rotation = qrot;


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
            controller.Move(move * edata.force * Time.deltaTime);
            controller.Move(transform.up * velocity * Time.deltaTime);
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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            GameController.instance._state = GameState.dead;
        }
    }
}
