using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] float smooth;
    [SerializeField] bool fullcontact;
    [SerializeField] float ClampRotationX;
    [SerializeField] float ClampRotationY;
    Transform turret;
    void Start()
    {
        turret = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion target_rot = Quaternion.LookRotation(target.transform.position - transform.position);
        if (!fullcontact)
        {
            target_rot.x = 0;
            target_rot.z = 0;
        }
        //calmp degli assi
        target_rot.x=Mathf.Clamp(target_rot.x, -ClampRotationX, ClampRotationX);//x
        target_rot.y = Mathf.Clamp(target_rot.y, -ClampRotationY, ClampRotationY);//y
        turret.rotation = Quaternion.Lerp(turret.rotation, target_rot, Time.deltaTime * smooth);
        
    }
}
