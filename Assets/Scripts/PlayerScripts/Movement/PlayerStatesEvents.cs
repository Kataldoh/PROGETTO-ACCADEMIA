using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesEvents : MonoBehaviour
{
    MainPlayerScript pInst = MainPlayerScript.pInstance;
    bool dmgTook = false;   //bool usato per prevenire che il danno preso non venga ripetuto
    float dashLifetime;
    float damageTimer, slideTimer, jumpTimer;
    int lockedDir = 0;
    float jumpDelay;
    public void P_Idle()
    {
        //Controlla se si può saltare in questo stato
        CanJump();


        if (pInst.isDash && pInst.dashUnlocked)
        {
            pInst._state = PlayerState.dash;
        }

        //Se vengono rilevati input e si è a terra con velocità minore-uguale a 0, si va al movimento base
        if(pInst.move != Vector3.zero && pInst.IsGrounded() && pInst.velocity <= 0)
        {
            pInst._state = PlayerState.groundMoving;
        }
        else if ((pInst.isJump && pInst.IsGrounded() && !pInst.hasSomethingAbove) || !pInst.IsGrounded())    //Se viene rilevato un salto e si è a terra oppure si sta cadendo
        {
            pInst._state = PlayerState.jump;
        }

        
    }

    public void P_Move()
    {
        CanJump();

        pInst.velocity = pInst.weight;                                                  //Applica il peso
        pInst.move.y = Mathf.Clamp(pInst.move.y, -1, 0);                                //Limita move.y a -1
        pInst.controller.Move(pInst.move * pInst.pdata.force * Time.deltaTime);         //Applica forza dagli input ricevuti
        pInst.controller.Move(pInst.transform.up * pInst.velocity * Time.deltaTime);    //Applica la velocity sull'asse y (Che sia gravità o salto)

        if(pInst.rollUnlocked)
        {
            if(pInst.move.y < 0)        //controlla l'altezza del player quando si alza e abbassa
            {
                pInst.controller.height = pInst.height/3;
                pInst.controller.center = new Vector3(0, 0.4f, 0);
            } 
            else                        //resetta l'altezza del player
            {
                pInst.controller.height = pInst.height;
                pInst.controller.center = new Vector3(0, 0.8f, 0);
            }
        }


        if (pInst.isDash && pInst.dashUnlocked)
        {
            pInst._state = PlayerState.dash;
        }
        //Se non vengono rilevati input e si è a terra con velocità minore-uguale a 0, si va ad idle
        if(pInst.move == Vector3.zero && pInst.IsGrounded() && pInst.velocity <= 0)
        {
            pInst._state = PlayerState.idle;
        }
        else if ((pInst.isJump && pInst.IsGrounded() && !pInst.hasSomethingAbove) || !pInst.IsGrounded())   //Se viene rilevato un salto e si è a terra oppure si sta cadendo
        {
            pInst._state = PlayerState.jump;
        }
        
    }

    public void P_Slide()
    {
        if (lockedDir == 0)
            lockedDir = pInst.dir;

        Vector3 dir = new Vector3(lockedDir, 0, 0);

        slideTimer += Time.deltaTime;

        if (slideTimer <= 0.5f && !pInst.hasSomethingInFront)
        {
            pInst.controller.height = pInst.height / 3;
            pInst.controller.center = new Vector3(0, 0.4f, 0);

            pInst.controller.Move(dir * pInst.pdata.force * 2 * Time.deltaTime);         //Applica forza dagli input ricevuti

            if (!pInst.IsGrounded())
            {
                lockedDir = 0;
                pInst._state = PlayerState.jump;
                pInst.controller.height = pInst.height;
                pInst.controller.center = new Vector3(0, 0.8f, 0);
            }
                
        }
        else
        {
            pInst.controller.height = pInst.height;
            pInst.controller.center = new Vector3(0, 0.8f, 0);

            if (!pInst.IsGrounded())
                pInst._state = PlayerState.jump;
            else
                pInst._state = PlayerState.idle;

            lockedDir = 0;
            slideTimer = 0;
            pInst.move.y = -1;
        }
    }


    public void P_Jump()
    {
        CanJump();

        if (pInst.isDash && pInst.dashUnlocked)
        {
            pInst._state = PlayerState.dash;
        }

        if (!pInst.hasSomethingAbove)
        {
            //Se viene rilevato un salto e si è a terra, applica la forza di salto alla velocity
            if (Input.GetButton("Jump") && pInst.IsGrounded() && pInst.isJump)
            {
                pInst.velocity = pInst.pdata.jumpForce * pInst.jumpArc.Evaluate(Time.deltaTime * pInst.pdata.jumpForce);
            }
            //Se il salto viene rilasciato in aria (Velocity>0), applica in anticipo la gravità
            //Permette salti di altezza variabile
            else if (!Input.GetButton("Jump") && pInst.velocity > 0)    
            {
                pInst.velocity += pInst.gravity * pInst.gravityArc.Evaluate(-Time.deltaTime * pInst.gravity / 2);
            }
        }
        else
        {
            pInst.velocity += pInst.gravity * pInst.gravityArc.Evaluate(-Time.deltaTime * pInst.gravity / 2);
        }



        //Se si è a terra con velocità minore-uguale a 0, metto la velocity al valore di peso, e torna in Idle
        if (pInst.IsGrounded() && pInst.velocity <= 0)
        {
            pInst.velocity = pInst.weight;
            pInst.isJump = false;
            pInst._state = PlayerState.idle;
        }

        //Se il salto viene rilasciato in aria (Velocity>0), applica in anticipo la gravità
        //Permette salti di altezza variabile
        if(!Input.GetButton("Jump") && pInst.velocity > 0)
        {
            pInst.velocity += pInst.gravity * pInst.gravityArc.Evaluate(-Time.deltaTime * pInst.gravity / 2);
        }

        
        pInst.controller.Move(pInst.transform.up * pInst.velocity * Time.deltaTime);    //Applica la velocity sull'asse y (Che sia gravità o salto)
        pInst.controller.Move(pInst.move * pInst.pdata.force * Time.deltaTime);         //Applica forza dagli input ricevuti

    }

    public void P_Dash()
    {
        if (lockedDir == 0)
            lockedDir = pInst.dir;

        Vector3 dir = new Vector3(lockedDir, 0, 0);

        dashLifetime += Time.deltaTime;

        if(dashLifetime <=0.25f && !pInst.hasSomethingInFront)
        {
            if(pInst.move.y == 0)
            {
                pInst.controller.Move(dir * pInst.pdata.force * 4 * Time.deltaTime);         //Applica forza dagli input ricevuti
            }
            else if(!pInst.IsGrounded())
            {
                pInst.controller.Move(-pInst.transform.up * pInst.pdata.force * 4 * Time.deltaTime);
            }
            
            GameController.instance.BarraStamina.SetStamina(0);
            pInst.velocity = 0;
        }
        else
        {
            if(!pInst.IsGrounded())
                pInst._state = PlayerState.jump;
            else
                pInst._state = PlayerState.idle;

            lockedDir = 0;
            dashLifetime = 0;
            pInst.dashTimer = 0;
            pInst.isDash = false;
            
        }
           

    }
    public void P_Damage()
    {
        pInst.controller.detectCollisions = false;
        //prendi danno solo se hai non hai preso del danno in questo stato
        //previene il ricevere danno continuo ogni ciclo
        if(!dmgTook)
        {
            GameController.instance.TakeDamage(10);
            dmgTook = true;
        }

        pInst.move.x = 0;
        
        damageTimer += Time.deltaTime;
        
        //Applica un knockback e gravità quando viene preso del danno
        float knockback = 1.5f;
        pInst.controller.Move(((pInst.transform.up * knockback) + (-pInst.transform.forward * knockback)) * Time.deltaTime);
        pInst.controller.Move(pInst.transform.up * pInst.velocity * Time.deltaTime);

        //Se a terra, torna in idle e resetta il danno
        if(pInst.IsGrounded() && damageTimer >= 0.15f)
        {
            damageTimer = 0;
            pInst.isInvincible = true;
            dmgTook = false;
            pInst._state = PlayerState.idle;
        }
    }   


    public void P_Death()
    {
        
    }

    public void CanJump()
    {
        //Uso un delay per fare in modo che il salto non sia costantemente attivato tenendo premuto il pulsante e garantendo che venga eseguito per più di un frame
        //Così da non rimanere a terra
        pInst.isJump = Input.GetButton("Jump");

        if (pInst.isJump)
        {
            jumpDelay += Time.deltaTime;
            if (jumpDelay < 0.1f)
            {
                pInst.isJump = true;
            }
            else
            {
                pInst.isJump = false;
            }
        }
        else
        {
            jumpDelay = 0;
        }
    }
    
 
}
