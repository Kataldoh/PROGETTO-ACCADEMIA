using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrawler_V2 : EnemyScript
{
    [SerializeField] Transform[] groundRaycast;
    [SerializeField] Transform frontRaycast;
    [SerializeField] GameObject rotateMesh;
    [SerializeField] bool hasTurned;
    public override void States()
    {
        switch (_state)
        {
            case EnemyState.patrol:
                GetComponent<Collider>().enabled = true;
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
        RaycastHit hit, hit2, hit3;
        bool GroundCast = Physics.Raycast(groundRaycast[0].position, -transform.up, out hit, 0.5f, layer);
        bool GroundCast2 = Physics.Raycast(groundRaycast[1].position, -transform.up, out hit2, 0.5f, layer);
        bool WallCast = Physics.Raycast(frontRaycast.position, transform.right, out hit3, 0.2f, layer);
        if (!GroundCast && !GroundCast2)
        {
            if (!hasTurned)
            {
                transform.Rotate(0, 0, -90);
            }
            hasTurned = true;
        }
        else
            hasTurned = false;

        if (WallCast)
            transform.Rotate(0, 0, 90);


        controller.Move(transform.right * edata.force);
    }

    void Dead()
    {
        
    }
    public override void StatelessChecks()
    {
        //no checks needed
    }
}
