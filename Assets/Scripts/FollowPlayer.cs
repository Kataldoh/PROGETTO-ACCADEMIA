using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothdamp;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newoffset = target.position - offset;
        Vector3 newpos = Vector3.Lerp(transform.position, newoffset, Time.deltaTime * smoothdamp);
      //  transform.LookAt(target);
        transform.position = newpos;
    }
}
