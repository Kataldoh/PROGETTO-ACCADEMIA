using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torretta : MonoBehaviour
{
    public Transform target;
    public GameObject parteDaRuotare;

    void Update()
    {
        
        parteDaRuotare.transform.LookAt(target);

    }

}
