using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesEvents : MonoBehaviour
{
    MainPlayerScript pInst = MainPlayerScript.pInstance;
    bool dmgTook = false;   //bool usato per prevenire che il danno preso non venga ripetuto
    float dashLifetime;
    public void P_Idle()
    {
        //Controlla se si può saltare in questo stato
        pInst.isJump = Input.GetButton("Jump");


        if (pInst.isDash && pInst.dashUnlocked)
        {
            pInst._state = PlayerState.dash;
        }

        //Se vengono rilevati input e si è a terra con velocità minore-uguale a 0, si va al movimento base
        if(pInst.move != Vector3.zero && pInst.IsGrounded() && pInst.velocity <= 0)
        {
            pInst._state = PlayerState.groundMoving;
        }
        else if ((pInst.isJump && pInst.IsGrounded())|| !pInst.IsGrounded())    //Se viene rilevato un salto e si è a terra oppure si sta cadendo
        {
            pInst._state = PlayerState.jump;
        }

        
    }

    public void P_Move()
    {
        pInst.isJump = Input.GetButton("Jump");
        //pInst.isSprinting = Input.GetKey(KeyCode.LeftShift);
        //pInst.speed = 60;

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
                pInst.controller.center = new Vector3(0,0.9f, 0);
            }
        }
        

        /*
        if (pInst.isSprinting)
        {
            pInst._state = PlayerState.sprinting;
        }
        */
       
        
        if(pInst.isDash && pInst.dashUnlocked)
        {
            pInst._state = PlayerState.dash;
        }
        //Se non vengono rilevati input e si è a terra con velocità minore-uguale a 0, si va ad idle
        if(pInst.move == Vector3.zero && pInst.IsGrounded() && pInst.velocity <= 0)
        {
            pInst._state = PlayerState.idle;
        }
        else if ((pInst.isJump && pInst.IsGrounded()) || !pInst.IsGrounded())   //Se viene rilevato un salto e si è a terra oppure si sta cadendo
        {
            pInst._state = PlayerState.jump;
        }
        
    }


    public void P_Jump()
    {
        pInst.isJump = Input.GetButton("Jump");

        if(pInst.isDash && pInst.dashUnlocked)
        {
            pInst._state = PlayerState.dash;
        }

        //Se viene rilevato un salto e si è a terra, applica la forza di salto alla velocity
        if(pInst.isJump && pInst.IsGrounded())
        {
            pInst.velocity = pInst.pdata.jumpForce;
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
        if(!pInst.isJump && pInst.velocity > 0)
        {
            pInst.velocity += pInst.gravity * Time.deltaTime;
        }

        
        pInst.controller.Move(pInst.transform.up * pInst.velocity * Time.deltaTime);    //Applica la velocity sull'asse y (Che sia gravità o salto)
        pInst.controller.Move(pInst.move * pInst.pdata.force * Time.deltaTime);         //Applica forza dagli input ricevuti

    }

    public void P_Dash()
    {
        Vector3 dir = new Vector3(pInst.dir, 0, 0);
        dashLifetime += Time.deltaTime;

        if(dashLifetime <=0.25f)
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

            dashLifetime = 0;
            pInst.dashTimer = 0;
            pInst.isDash = false;
            
        }
           

    }
    public void P_Damage()
    {
        //prendi danno solo se hai non hai preso del danno in questo stato
        //previene il ricevere danno continuo ogni ciclo
        if(!dmgTook)
        {
            GameController.instance.TakeDamage(10);
            dmgTook = true;
        }

        //Applica un knockback e gravità quando viene preso del danno
        float knockback = 1.5f;
        pInst.controller.Move(((pInst.transform.up * knockback) + (-pInst.transform.forward * knockback * pInst.dir)) * Time.deltaTime);
        pInst.controller.Move(pInst.transform.up * pInst.velocity * Time.deltaTime);

        //Se a terra, torna in idle e resetta il danno
        if(pInst.IsGrounded())
        {
            dmgTook = false;
            pInst._state = PlayerState.idle;
        }
    }   

    /*
    public void P_Sprinting()
    {
        pInst.speed = 100;

        pInst.velocity = pInst.weight;                                                  //Applica il peso
        pInst.move.y = Mathf.Clamp(pInst.move.y, -1, 0);                                //Limita move.y a -1
        pInst.controller.Move(pInst.move * pInst.pdata.force * Time.deltaTime);         //Applica forza dagli input ricevuti
        pInst.controller.Move(pInst.transform.up * pInst.velocity * Time.deltaTime);    //Applica la velocity sull'asse y (Che sia gravità o salto)

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            pInst.isSprinting = false;
        }

        if (pInst.isSprinting == false)
        {
            pInst._state = PlayerState.groundMoving;
            
        }

        pInst.isJump = Input.GetButton("Jump");

        if (pInst.move == Vector3.zero && pInst.IsGrounded() && pInst.velocity <= 0)
        {
            pInst._state = PlayerState.idle;
        }
        else if ((pInst.isJump && pInst.IsGrounded()) || !pInst.IsGrounded())   //Se viene rilevato un salto e si è a terra oppure si sta cadendo
        {
            pInst._state = PlayerState.jump;
        }

    }
    */

    public void P_Death()
    {
        
    }
 
}
