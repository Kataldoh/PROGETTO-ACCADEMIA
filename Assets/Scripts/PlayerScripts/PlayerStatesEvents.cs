using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesEvents : MonoBehaviour
{
    MainPlayerScript pInst = MainPlayerScript.pInstance;
    float hangTime;
    public void P_Idle()
    {
        pInst.isJump = Input.GetButton("Fire1");
        if(pInst.move != Vector3.zero && pInst.IsGrounded() && pInst.velocity <= 0)
        {
            pInst._state = PlayerState.groundMoving;
        }
        else if ((pInst.isJump && pInst.IsGrounded())|| !pInst.IsGrounded())
        {
            pInst._state = PlayerState.jump;
        }
    }
    public void P_Move()
    {
        pInst.isJump = Input.GetButton("Fire1");
        pInst.move.y = Mathf.Clamp(pInst.move.y, -1, 0);
        pInst.controller.Move(pInst.move * pInst.pdata.force * Time.deltaTime);
        pInst.controller.Move(pInst.transform.up * pInst.velocity * Time.deltaTime);
        // transform.Translate(move);

        if ((pInst.isJump && pInst.IsGrounded()) || !pInst.IsGrounded())
        {
            hangTime += Time.deltaTime;
            if(hangTime >= 0.1f)
            {
                pInst._state = PlayerState.jump;
                hangTime = 0;
            }
                
        }
        else if(pInst.move == Vector3.zero)
        {
            pInst._state = PlayerState.idle;
        }
    }
    public void P_Jump()
    {
        pInst.isJump = Input.GetButton("Fire1");

        if(pInst.isJump && pInst.IsGrounded())
        {
            pInst.velocity = pInst.pdata.jumpForce;
        }
        
        if (pInst.IsGrounded() && pInst.velocity <= 0)
        {
            pInst.velocity = pInst.weight;
            pInst.isJump = false;
            pInst._state = PlayerState.idle;
        }

        
        pInst.controller.Move(pInst.transform.up * pInst.velocity * Time.deltaTime);
        pInst.controller.Move(pInst.move * pInst.pdata.force * Time.deltaTime);
        
    }
    public void P_Damage(){}
    public void P_Death(){}
 
}
