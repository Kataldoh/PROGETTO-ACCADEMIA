using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesEvents : MonoBehaviour
{
    MainPlayerScript pInst = MainPlayerScript.pInstance;
    float hangTime; //(da implementare) un hangtime per dare al giocatore una finestra per saltare dopo essere in aria
    bool dmgTook = false;   //bool usato per prevenire che il danno preso non venga ripetuto
    public void P_Idle()
    {
        //Controlla se si può saltare in questo stato
        pInst.isJump = Input.GetButton("Fire1");

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
        pInst.isJump = Input.GetButton("Fire1");

        pInst.velocity = pInst.weight;                                                  //Applica il peso
        pInst.move.y = Mathf.Clamp(pInst.move.y, -1, 0);                                //Limita move.y a -1
        pInst.controller.Move(pInst.move * pInst.pdata.force * Time.deltaTime);         //Applica forza dagli input ricevuti
        pInst.controller.Move(pInst.transform.up * pInst.velocity * Time.deltaTime);    //Applica la velocity sull'asse y (Che sia gravità o salto)
        
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
        pInst.isJump = Input.GetButton("Fire1");

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
    public void P_Death()
    {
        
    }
 
}
