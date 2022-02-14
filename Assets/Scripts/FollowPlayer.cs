using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //https://www.youtube.com/watch?v=OD-awL_4G3E

    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothdamp;


    
    void FixedUpdate() // camera che segue il player
    {
        cameraFollow();
    }

    public void cameraFollow() //camera che segue il player in modo smooth
    {
        Vector3 newoffset = target.position - offset;
        Vector3 newpos = Vector3.Lerp(transform.position, newoffset, Time.deltaTime * smoothdamp);
        //  transform.LookAt(target);
        transform.position = newpos;
    }

    /*
    public void cameraScatto() //camera che non segue il player - in stile metroid classico
    {
        if(target.transform.position.x >= transform.position.x + 9)
        {
            transform.position = new Vector3(transform.position.x + 18, 0, -10);
        }

        if (target.transform.position.x <= transform.position.x - 9)
        {
            transform.position = new Vector3(transform.position.x - 18, 0, -10);
        }
    }
    */
}
