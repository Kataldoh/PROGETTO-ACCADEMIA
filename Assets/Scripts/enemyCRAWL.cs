using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCRAWL : MonoBehaviour
{
    public float movespeed;
    public GameObject[] nodi;

    int nodosuccessivo = 1;
    float distanzanodi;

     void Update()
    {
        Move();
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
