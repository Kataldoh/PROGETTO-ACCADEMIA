using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrawler_V2 : EnemyScript
{
    [SerializeField] Transform crawlingRaycasts;
    Quaternion qrot;
    bool spawnedWithNoContact = true;
    public override void States()
    {
        switch (_state)
        {
            case EnemyState.patrol:
                Patrol();
                break;
            case EnemyState.dead:
                if (!isDead)
                    Dead();
                break;
        }
    }

    public override void Patrol()
    {
        Debug.DrawRay(crawlingRaycasts.position, transform.up * 0.3f, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(crawlingRaycasts.position, -transform.up, out hit, 0.3f, layer))
        {
            controller.Move(transform.right * edata.force);
            float normalAngle = Vector3.Angle(hit.normal, Vector3.up);
            qrot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, normalAngle), Time.deltaTime * edata.speedRot);
            transform.rotation = qrot;
        }
        /*
        else 
        {
            transform.Rotate(new Vector3(0, 0, -2));
        }
        */

    }
    public override void StatelessChecks()
    {
        //no checks needed
    }
}
