using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torretta : MonoBehaviour
{
                                                             
    public Transform target;//target sarebbe il player 
    public GameObject parteDaRuotare;// distanza dall'area di attacco della torretta
    public float attackarea;
    Vector3 rotazioneIniziale;
    public float smooth;

    float timebeetweenshots; //tempo totale dell'intervallo totale
    public float starttimebetweenshots;// e' il tempo tra un proiettile e l'altro
    public GameObject meshproiettile;
    public Transform foro;

    
    public int rafficacolpi;
    int contacolpi = 0;
    
    float timer;
    bool reload;
    public float tempo;
    private void Start()
    {
        
        //rotazioneIniziale = new Vector3(0, 180, 0);
        timebeetweenshots = starttimebetweenshots;
    }
    void Update()
    {
        // verifico se il tempo � 0 e la booleana � vera
        if(timer<=0 && reload)
        {
            //falso perche l'impedisce di entrare nel metodo sparo prima del timer
            reload = false;
        }
        // timer minore di 3 e booleana � falsa
        if (timer<=tempo && !reload)
        {
            sparo();
            timer += Time.deltaTime;// incremento la variabile timer
        }
        else //se reload=true 
        {
            //attendo tot secondi prima del prossimo intervallo di colpi
           new WaitForSeconds(tempo);
            timer -= Time.deltaTime;
            reload = true;
        }
        
    }


    void smoothRotation() {
        Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, target.transform.position.z);
        Quaternion target_rot = Quaternion.LookRotation(targetPos - transform.position);
        //target_rot.x = 0;
        //target_rot.z = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, target_rot, Time.deltaTime * smooth);
    }


    void sparo()
    {
        // se la distanza tra il player e la torretta supera la distanza di attacco allora la torretta comincia a ruotare e sparare  individuando la posizione del player
        if (Vector3.Distance(transform.position, target.position) < attackarea)
        {
            //parteDaRuotare.transform.LookAt(target);

            smoothRotation();

            // se il tempo tra uno sparo e l'altro e' minore o uguale a zero allora la torreta genera un proiettile
            if (timebeetweenshots <= 0)
            {
                if (contacolpi <= rafficacolpi)
                {
                    //istanzio quello che devo generare( ovvero il proiettile), la sua posizione e rotazione nello spazio(in questo caso non ruota)
                    Instantiate(meshproiettile, foro.position, foro.rotation);
                    contacolpi++;
                   
                    timebeetweenshots = starttimebetweenshots;// se non faccio cosi, la torretta sparerebbe un proiettile ad ogni frame

                }
            }
            else
            {
                contacolpi = 0;
                timebeetweenshots -= Time.deltaTime;
            }

        }
        else
        {
            //Quaternion(Quaternione) e' un sistema  usato per rappresentare le rotazioni  ,,,  Quaternion.Euler= restituisce una rotazione lungo gli assi x,y,z 
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * smooth);
        }

    }


}
