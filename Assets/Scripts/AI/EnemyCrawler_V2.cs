using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrawler_V2 : EnemyScript
{
    [SerializeField] Transform[] groundRaycast;
    [SerializeField] Transform frontRaycast;
    int rotateDir;
    public override void StatesNOPhys()
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
        Debug.DrawRay(groundRaycast[0].position, -transform.up * 0.75f, Color.red);
        Debug.DrawRay(frontRaycast.position, transform.right * 0.25f, Color.blue);
        bool GroundCast = Physics.Raycast(groundRaycast[0].position, -transform.up, out hit, 0.75f, layer);
        //bool GroundCast2 = Physics.Raycast(groundRaycast[1].position, -transform.up, out hit2, 0.2f, layer);
        bool WallCast = Physics.Raycast(frontRaycast.position, transform.right, out hit3, 0.25f, layer);
        if (!GroundCast)
        {
            /*
            if (!hasTurned)
            {
                transform.Rotate(0, 0, -90);
                hasTurned = true;
            }
            */
            transform.Rotate(0, 0, -0.2f);

        }
        else
        {
            AllignToNormal(transform, hit);
            transform.Translate(Vector3.right * edata.force * Time.deltaTime, Space.Self);
        }

        if (WallCast)
        {
            transform.Rotate(0, 0, 90f);
            //AllignToNormal(transform, hit3);
        }

        //Vector3 moveDir = MoveDir();

        
    }

    void AllignToNormal(Transform t, RaycastHit hit)
    {
        Quaternion newrot = Quaternion.FromToRotation(t.up, hit.normal) * t.rotation;
        float angle = Vector3.Angle(t.up, hit.normal);
        t.rotation = Quaternion.RotateTowards(t.rotation, newrot, angle * Time.deltaTime * edata.speedRot);
    }


    public override void Dead()
    {
        
    }

    /*
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Nemico")
        {
            transform.Rotate(0, 180, 0);
        }
    }
    */

    public override void States()
    {
        //No physics/FixedUpdate needed
    }
    public override void StatelessChecks()
    {
        //no checks needed
    }
}
