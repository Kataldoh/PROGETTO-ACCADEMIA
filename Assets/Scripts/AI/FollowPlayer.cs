using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //https://www.youtube.com/watch?v=OD-awL_4G3E


    [SerializeField] Transform targetPlayer;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothdamp;


    
    void FixedUpdate() // camera che segue il player
    {
        cameraFollow();
    }

    public void cameraFollow() //camera che segue il player in modo smooth
    {

        offset = GameController.instance.cameraOffset;
        smoothdamp = GameController.instance.cameraSmoothing;

        Vector3 cameraTarget = Vector3.zero;
        if (GameController.instance.followPlayerX) 
        {
            cameraTarget.x = targetPlayer.position.x;
        }
        else
        {
            cameraTarget.x = GameController.instance.triggerPos.x;
        }

        if (GameController.instance.followPlayerY)
        {
            cameraTarget.y = targetPlayer.position.y;
        }
        else
        {
            cameraTarget.y = GameController.instance.triggerPos.y;
        }

        cameraTarget.z = targetPlayer.position.z;


        Vector3 newoffset = cameraTarget - offset;
        Vector3 newpos = Vector3.Lerp(transform.position, newoffset, Time.deltaTime * smoothdamp);
        //  transform.LookAt(target);
        transform.position = newpos;
    }

    public void cameraScatto() //camera che non segue il player - in stile metroid classico
    {
        if(targetPlayer.transform.position.x >= transform.position.x + 9)
        {
            transform.position = new Vector3(transform.position.x + 18, 0, -10);
        }

        if (targetPlayer.transform.position.x <= transform.position.x - 9)
        {
            transform.position = new Vector3(transform.position.x - 18, 0, -10);
        }
    }
}
