using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torretta : MonoBehaviour
{
    //player
    public Transform target;
    public GameObject parteDaRuotare;
    // distanza dall'area di attacco della torretta
    public float attackarea;

    void Update()
    {
        // se la distanza tra il player e la torretta supera la distanza di attacco allora la torretta comincia a ruotare individuando la posizione del player
        if (Vector3.Distance(transform.position, target.position) < attackarea)
        {
            parteDaRuotare.transform.LookAt(target);
            print(target.position);
        }
        

    }

}
