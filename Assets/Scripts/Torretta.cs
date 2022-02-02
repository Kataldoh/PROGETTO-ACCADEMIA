using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torretta : MonoBehaviour
{
    public Transform target;
    public float speed;

    void Update()
    {
        transform.LookAt(target);
       

    }

}
