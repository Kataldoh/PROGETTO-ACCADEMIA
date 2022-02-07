using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torretta : MonoBehaviour
{
                                                             
    public Transform target;//target sarebbe il player 
    public GameObject parteDaRuotare;// distanza dall'area di attacco della torretta
    public float attackarea;
    Vector3 rotazioneIniziale;

    private float timebeetweenshots;
    public float starttimebetweenshots;// e' il tempo tra un proiettile e l'altro
    public GameObject meshproiettile;

    private void Start()
    {
        rotazioneIniziale = new Vector3(0, 180, 0);
        timebeetweenshots = starttimebetweenshots;
    }
    void Update()
    {
        // se la distanza tra il player e la torretta supera la distanza di attacco allora la torretta comincia a ruotare e sparare  individuando la posizione del player
        if (Vector3.Distance(transform.position, target.position) < attackarea)
        {
            parteDaRuotare.transform.LookAt(target);

            // se il tempo tra uno sparo e l'aktro e' minore o uguale a zero allora la torreta genera un proiettile
            if (timebeetweenshots <= 0)
            {
                //istanzio quello che devo generare( ovvero il proiettile), la sua posizione e rotazione nello spazio(in questo caso non ruota)
                Instantiate(meshproiettile, transform.position, Quaternion.identity);
                timebeetweenshots = starttimebetweenshots;// se non faccio cosi, la torretta sparerebbe un proiettile ad ogni frame
            }
            else 
            {
                
                timebeetweenshots -= Time.deltaTime;
            }

        }
        else
        {
            //Quaternion(Quaternione) e' un sistema  usato per rappresentare le rotazioni  ,,,  Quaternion.Euler= restituisce una rotazione lungo gli assi x,y,z 
            transform.rotation = Quaternion.Euler(rotazioneIniziale);
        }
        
        
    }

}
