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
    Vector3 rotazioneIniziale;

    private float timebeetweenshots;
    public float starttimebetweenshots;
    public GameObject meshproiettile;

    private void Start()
    {
        rotazioneIniziale = new Vector3(0, 180, 0);
        timebeetweenshots = starttimebetweenshots;
    }
    void Update()
    {
        // se la distanza tra il player e la torretta supera la distanza di attacco allora la torretta comincia a ruotare individuando la posizione del player
        if (Vector3.Distance(transform.position, target.position) < attackarea)
        {
            parteDaRuotare.transform.LookAt(target);
            
        }
        else
        {
            //Quaternion(Quaternione) e' un sistema  usato per rappresentare le rotazioni  ,,,  Quaternion.Euler= restituisce una rotazione lungo gli assi x,y,z 
            transform.rotation = Quaternion.Euler(rotazioneIniziale);
        }
        
        if(timebeetweenshots <= 0)
        {
            Instantiate(meshproiettile, transform.position, Quaternion.identity);
            timebeetweenshots = starttimebetweenshots;
        }
        else
        {
            timebeetweenshots -= Time.deltaTime;
        }
    }

}
