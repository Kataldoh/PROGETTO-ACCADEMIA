using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCRAWL : MonoBehaviour
{
    public float movespeed;
    public GameObject[] nodi;

    [SerializeField] HealthPlaceholder hp;

    int nodosuccessivo = 1;
    float distanzanodi;

    //parte per animazioni
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(hp.health <= 0) // se la sua vita si azzera, l'oggetto attiva l'animazione di distruzione
        {
            Destroy(this.gameObject);
        }
        
        Move();
        //anim.Play("Idle");
    }

     void Move()
    {
        distanzanodi = Vector3.Distance(transform.position, nodi[nodosuccessivo].transform.position);
        transform.position = Vector3.MoveTowards(transform.position, nodi[nodosuccessivo].transform.position,
            movespeed * Time.deltaTime);

        if (distanzanodi < 0.2f)
        {
            TakeTurn();
        }

        void TakeTurn()
        {
            Vector3 rotation = transform.eulerAngles;
            rotation.x = nodi[nodosuccessivo].transform.eulerAngles.x;
            transform.eulerAngles = rotation;

            NextNodo();

        }

        void NextNodo()
        {
            nodosuccessivo++;

            if (nodosuccessivo == nodi.Length)
            {
                nodosuccessivo = 0;
            }
        }
    }
}
