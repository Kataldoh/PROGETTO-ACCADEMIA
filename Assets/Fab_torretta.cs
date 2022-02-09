using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fab_torretta : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    public float smooth;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion target_rot = Quaternion.LookRotation(target.transform.position - transform.position);
        target_rot.x = 0;
        target_rot.z = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, target_rot, Time.deltaTime * smooth);
    }
}
