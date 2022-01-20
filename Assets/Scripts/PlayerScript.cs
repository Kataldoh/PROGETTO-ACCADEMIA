using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{


    public int maxHealth = 100;
    public int CurrentHealth;
    public Barra BarraVita;

    public PlayerData pdata; // SCRIPTABLE OBJECT che determina forza del salto,velocità della rotazione,lunghezza del raycast frontale

    public PlayerState _state; // Stati del player

    Vector3 move;
    [SerializeField] float force;
    CharacterController controller;
    [SerializeField] bool isGrounded; //bool che determina se il player è a terra oppure no
    bool isJump;
    [SerializeField] Transform foot;
    [SerializeField] Transform rayhead;
    [SerializeField] LayerMask layer;
    [SerializeField] Animator anim;

    //*****************************************
    [SerializeField] float JumpForce;
    [SerializeField] float gravity;
    [SerializeField] float Wheight;
    [SerializeField] float rayLenght;
    [SerializeField] Transform cursor;
    float velocity;
    float rot=90;
    int dir;
    [SerializeField] float speedRot;

    void Start()
    {
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }


    void FixedUpdate()
    {
      


        if (GameController.instance._state == GameState.play)
        {

            //Questo if serve solo per testare se il metodo TakeDamage funziona (Aldo)
          

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameController.instance._state = GameState.pause;
            }

            //movimento
            move = new Vector3(
                   Input.GetAxis("Horizontal") * Time.deltaTime * 64,
                   Input.GetAxis("Vertical"),
                   0
                );
            isJump = Input.GetButton("Fire1");

            isGrounded = IsGrounded();

            if (isJump && IsGrounded() && !IsInRayCastDireciton(transform.up, 0.5f, layer) || (isJump && IsInRayCastDireciton(transform.forward, pdata.raycastForward, layer)))
            {
                velocity = pdata.jumpForce;
                anim.SetBool("jump", true);

            }
            else if (IsGrounded() && velocity < 0)
            {
                velocity = Wheight;
                anim.SetBool("jump", false);
                isJump = false;
            }
            else
            {
                velocity += gravity * Time.deltaTime;
            }


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


            var direction = (transform.position - cursor.position).normalized;
            //print(transform.position - cursor.position);
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //print(angle);

            //rotazione del player
            Quaternion qrot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rot, 0), Time.deltaTime * pdata.speedRot);
            transform.rotation = qrot;

            move.y = Mathf.Clamp(move.y, -1, 0);
            anim.SetFloat("posx", move.x, 0.05f, Time.deltaTime);
            anim.SetFloat("posy", move.y, 0.15f, Time.deltaTime);
            controller.Move(move * pdata.force * Time.deltaTime);
            controller.Move(transform.up * velocity * Time.deltaTime);
            // transform.Translate(move);
        }
    }

    bool IsInRayCastDireciton(Vector3 direction, float lenght, LayerMask layer) {
        Debug.DrawRay(rayhead.position, direction * lenght, Color.red);
        return Physics.Raycast(rayhead.position, direction, out RaycastHit hit, lenght, layer);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(foot.position, rayLenght, layer);
    }

    void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        BarraVita.SetHealth(CurrentHealth);
    }
}
