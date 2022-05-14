using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrawler_V2 : EnemyScript
{
    [SerializeField] Transform groundRaycast;
    [SerializeField] Transform frontRaycast;
    [SerializeField] Transform fallRaycast;
    [SerializeField] bool startTurning;
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
        Debug.DrawRay(groundRaycast.position, transform.up * 0.5f, Color.red);
        RaycastHit hit, hit2;
        bool GroundCast = Physics.Raycast(groundRaycast.position, -transform.up, out hit, 0.5f, layer);
        bool FallCast = Physics.Raycast(fallRaycast.position, -transform.up, out hit2, 0.5f, layer);
        if (GroundCast || FallCast)
        {
            controller.Move(-transform.right * edata.force);
            float normalAngle = Vector3.Angle(hit.normal, Vector3.up);
            Quaternion qrot;
            qrot = Quaternion.Euler(0, 0, normalAngle);
            transform.rotation = qrot;
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 2));
        }
        

    }
    public override void StatelessChecks()
    {
        //no checks needed
    }
}
